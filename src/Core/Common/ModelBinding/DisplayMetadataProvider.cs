namespace GamaEdtech.Common.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Resources;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

    public class DisplayMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var attributes = context.Attributes;
            var dataTypeAttribute = attributes.OfType<DataTypeAttribute>().FirstOrDefault();
            var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            var displayColumnAttribute = attributes.OfType<System.ComponentModel.DataAnnotations.DisplayColumnAttribute>().FirstOrDefault();
            var displayFormatAttribute = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
            var displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            var hiddenInputAttribute = attributes.OfType<Microsoft.AspNetCore.Mvc.HiddenInputAttribute>().FirstOrDefault();
            var scaffoldColumnAttribute = attributes.OfType<ScaffoldColumnAttribute>().FirstOrDefault();

            var elementName = context.Key.Name;

            // Special case the [DisplayFormat] attribute hanging off an applied [DataType] attribute. This property is
            // non-null for DataType.Currency, DataType.Date, DataType.Time, and potentially custom [DataType]
            // subclasses. The DataType.Currency, DataType.Date, and DataType.Time [DisplayFormat] attributes have a
            // non-null DataFormatString and the DataType.Date and DataType.Time [DisplayFormat] attributes have
            // ApplyFormatInEditMode==true.
            if (displayFormatAttribute is null && dataTypeAttribute is not null)
            {
                displayFormatAttribute = dataTypeAttribute.DisplayFormat;
            }

            var displayMetadata = context.DisplayMetadata;

            // ConvertEmptyStringToNull
            if (displayFormatAttribute is not null)
            {
                displayMetadata.ConvertEmptyStringToNull = displayFormatAttribute.ConvertEmptyStringToNull;
            }

            // DataTypeName
            if (dataTypeAttribute is not null)
            {
                displayMetadata.DataTypeName = dataTypeAttribute.GetDataTypeName();
            }
            else if (displayFormatAttribute is not null && !displayFormatAttribute.HtmlEncode)
            {
                displayMetadata.DataTypeName = nameof(ElementDataType.Html);
            }

            ResourceManager? manager = null;
            if (displayAttribute?.ResourceType is not null)
            {
                manager = new ResourceManager(displayAttribute.ResourceType);
            }
            else if (displayAttribute?.ResourceTypeName is not null)
            {
                var type = Type.GetType(displayAttribute.ResourceTypeName) ?? throw new ArgumentException(nameof(DisplayAttribute.ResourceTypeName));

                manager = new ResourceManager(type);
            }

            if (manager is null && context.Key.ContainerType is not null)
            {
                var name = context.Key.ContainerType.AssemblyQualifiedName.PrepareResourcePath();
                if (!string.IsNullOrEmpty(name))
                {
                    var type = Type.GetType(name);
                    if (type is not null)
                    {
                        manager = new ResourceManager(type);
                    }
                }
            }

            // Description
            if (displayAttribute is not null)
            {
                displayMetadata.Description = () => Globals.GetLocalizedValueInternal(displayAttribute, elementName!, Constants.ResourceKey.Description, manager) ?? elementName;
            }

            // DisplayFormatString
            if (displayFormatAttribute is not null)
            {
                displayMetadata.DisplayFormatString = displayFormatAttribute.DataFormatString;
            }

            // DisplayName
            // DisplayAttribute has precedence over DisplayNameAttribute.
            if (displayAttribute is not null)
            {
                displayMetadata.DisplayName = () => Globals.GetLocalizedValueInternal(displayAttribute, elementName!, Constants.ResourceKey.Name, manager) ?? elementName;
            }
            else if (displayNameAttribute is not null)
            {
                displayMetadata.DisplayName = () => displayNameAttribute.DisplayName;
            }

            // EditFormatString
            if (displayFormatAttribute is not null && displayFormatAttribute.ApplyFormatInEditMode)
            {
                displayMetadata.EditFormatString = displayFormatAttribute.DataFormatString;
            }

            // IsEnum et cetera
            var underlyingType = Nullable.GetUnderlyingType(context.Key.ModelType) ?? context.Key.ModelType;
            if (underlyingType.IsEnum)
            {
                // IsEnum
                displayMetadata.IsEnum = true;

                // IsFlagsEnum
                displayMetadata.IsFlagsEnum = underlyingType.IsDefined(typeof(FlagsAttribute), false);

                // EnumDisplayNamesAndValues and EnumNamesAndValues
                //
                // Order EnumDisplayNamesAndValues by DisplayAttribute.Order, then by the order of Enum.GetNames().
                // That method orders by absolute value, then its behavior is undefined (but hopefully stable).
                // Add to EnumNamesAndValues in same order but Dictionary does not guarantee order will be preserved.
                var groupedDisplayNamesAndValues = new List<KeyValuePair<EnumGroupAndName, string>>();
                var namesAndValues = new Dictionary<string, string>();

                var enumFields = Enum.GetNames(underlyingType).Select(underlyingType.GetField).OrderBy(t => t?.GetCustomAttribute<DisplayAttribute>(false)?.Order);

                foreach (var field in enumFields)
                {
                    if (field is null)
                    {
                        continue;
                    }

                    var groupName = Globals.GetLocalizedGroupName(field) ?? string.Empty;
                    var value = (field.GetValue(null) as Enum)?.ToString("d")!;

                    groupedDisplayNamesAndValues.Add(new KeyValuePair<EnumGroupAndName, string>(
                        new EnumGroupAndName(
                            groupName,
                            () => Globals.GetLocalizedDisplayName(field)!),
                        value));
                    namesAndValues.Add(field.Name, value);
                }

                displayMetadata.EnumGroupedDisplayNamesAndValues = groupedDisplayNamesAndValues;
                displayMetadata.EnumNamesAndValues = namesAndValues;
            }

            // HasNonDefaultEditFormat
            if (!string.IsNullOrEmpty(displayFormatAttribute?.DataFormatString) &&
                displayFormatAttribute?.ApplyFormatInEditMode == true)
            {
                var displayFormat = dataTypeAttribute is null || dataTypeAttribute.DisplayFormat != displayFormatAttribute;

                // Have a non-empty EditFormatString based on [DisplayFormat] from our cache.
                if (displayFormat || dataTypeAttribute is null)
                {
                    // Attributes include no [DataType]; [DisplayFormat] was applied directly.
                    displayMetadata.HasNonDefaultEditFormat = true;
                }
            }

            // HideSurroundingHtml
            if (hiddenInputAttribute is not null)
            {
                displayMetadata.HideSurroundingHtml = !hiddenInputAttribute.DisplayValue;
            }

            // HtmlEncode
            if (displayFormatAttribute is not null)
            {
                displayMetadata.HtmlEncode = displayFormatAttribute.HtmlEncode;
            }

            // NullDisplayText
            if (displayFormatAttribute is not null)
            {
                displayMetadata.NullDisplayText = displayFormatAttribute.NullDisplayText;
            }

            // Order
            if (displayAttribute is not null)
            {
                displayMetadata.Order = displayAttribute.Order;
            }

            // Placeholder
            if (displayAttribute is not null)
            {
                displayMetadata.Placeholder = () => Globals.GetLocalizedValueInternal(displayAttribute, elementName!, Constants.ResourceKey.Prompt, manager) ?? elementName;
            }

            // ShowForDisplay
            if (scaffoldColumnAttribute is not null)
            {
                displayMetadata.ShowForDisplay = scaffoldColumnAttribute.Scaffold;
            }

            // ShowForEdit
            if (scaffoldColumnAttribute is not null)
            {
                displayMetadata.ShowForEdit = scaffoldColumnAttribute.Scaffold;
            }

            // SimpleDisplayProperty
            if (displayColumnAttribute is not null)
            {
                displayMetadata.SimpleDisplayProperty = displayColumnAttribute.DisplayColumn;
            }

            // TemplateHint
            if (hiddenInputAttribute is not null)
            {
                displayMetadata.TemplateHint = "HiddenInput";
            }
        }
    }
}

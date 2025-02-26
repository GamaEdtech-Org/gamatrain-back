namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute, IClientModelValidator
    {
        public StringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_StringLength);
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
        }

        public StringLengthAttribute(int maximumLength, int minimumLength)
            : base(maximumLength)
        {
            MinimumLength = minimumLength;
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_StringLength);
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
        }

        public new Type? ErrorMessageResourceType
        {
            get => base.ErrorMessageResourceType;

            private set => base.ErrorMessageResourceType = value;
        }

        public new string? ErrorMessageResourceName
        {
            get => base.ErrorMessageResourceName;

            private set => base.ErrorMessageResourceName = value;
        }

        public new string? ErrorMessage
        {
            get => base.ErrorMessage;

            internal set => base.ErrorMessage = value;
        }

        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString())
|| (value is List<string> lst ? lst.All(t => string.IsNullOrEmpty(t) || base.IsValid(t)) : base.IsValid(value));

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-length", Data.Error.FormatMessage(msg)));

            if (MaximumLength != int.MaxValue)
            {
                _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-length-max", MaximumLength.ToString()));
            }

            if (MinimumLength != 0)
            {
                _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-length-min", MinimumLength.ToString()));
            }
        }

        public override string FormatErrorMessage(string name) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinimumLength, MaximumLength);
    }
}

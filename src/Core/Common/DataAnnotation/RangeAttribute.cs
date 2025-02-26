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

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class RangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute, IClientModelValidator
    {
        public RangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Range);
        }

        public RangeAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Range);
        }

        public RangeAttribute(Type type, string minimum, string maximum)
            : base(type, minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Range);
            Type = type;
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

        public Type? Type { get; }

        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString())
|| (value is IEnumerable<string> lst ? lst.All(t => string.IsNullOrEmpty(t) || base.IsValid(t)) : base.IsValid(value));

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-max", Maximum.ToString()!));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-min", Minimum!.ToString()!));

            var msg = FormatMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-range", Data.Error.FormatMessage(msg)));
        }

        private string? FormatMessage(string? modelDisplayName) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, modelDisplayName, Minimum, Maximum);
    }
}

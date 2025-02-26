namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute, IClientModelValidator
    {
        public RequiredAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Required);
        }

        public new string? ErrorMessageResourceName
        {
            get => base.ErrorMessageResourceName;

            private set => base.ErrorMessageResourceName = value;
        }

        public new string? ErrorMessage => base.ErrorMessage;

        public new Type? ErrorMessageResourceType
        {
            get => base.ErrorMessageResourceType;

            private set => base.ErrorMessageResourceType = value;
        }

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-required", Data.Error.FormatMessage(msg)));
        }
    }
}

namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class ValidationAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        protected ValidationAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Expression);
        }

        protected ValidationAttribute(string errorMessage)
            : base(errorMessage)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Expression);
        }

        protected ValidationAttribute(Func<string> errorMessageAccessor)
            : base(errorMessageAccessor)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Expression);
        }

        public new Type? ErrorMessageResourceType
        {
            get => base.ErrorMessageResourceType;

            private set => base.ErrorMessageResourceType = value;
        }

        public new string? ErrorMessageResourceName
        {
            get => base.ErrorMessageResourceName;

            internal set => base.ErrorMessageResourceName = value;
        }

        public new string? ErrorMessage => base.ErrorMessage;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => string.IsNullOrEmpty(value?.ToString()) ? ValidationResult.Success : base.IsValid(value, validationContext);
    }
}

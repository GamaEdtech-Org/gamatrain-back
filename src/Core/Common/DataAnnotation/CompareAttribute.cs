namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Collections.Generic;
    using GamaEdtech.Common.Resources;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CompareAttribute : System.ComponentModel.DataAnnotations.CompareAttribute, IClientModelValidator
    {
        public CompareAttribute(string otherProperty)
            : base(otherProperty)
        {
            ErrorMessageResourceType = typeof(GlobalResource);
            ErrorMessageResourceName = nameof(GlobalResource.Validation_Compare);
        }

        public string? ValidatorPrefix { get; set; }

        public new string? OtherPropertyDisplayName { get; internal set; }

        public Constants.OperandType OperandType { get; set; } = Constants.OperandType.Equals;

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

        public override string FormatErrorMessage(string name) => FormatErrorMessage(name, null);

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-equalto-other", $"{ValidatorPrefix}.{OtherProperty}"));
            var key = string.Empty;
            switch (OperandType)
            {
                case Constants.OperandType.Equals:
                    key = "equalTo";
                    break;
                case Constants.OperandType.GreaterThan:
                    key = "greaterThan";
                    break;
                case Constants.OperandType.LessThan:
                    key = "lessThan";
                    break;
                case Constants.OperandType.GreaterThanOrEqual:
                    key = "greaterThanEqualTo";
                    break;
                case Constants.OperandType.LessThanOrEqual:
                    key = "lessThanEqualTo";
                    break;
                case Constants.OperandType.NotEquals:
                    key = "notEqualTo";
                    break;
            }

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata!.ContainerType!.GetProperty(context.ModelMetadata!.Name!)), Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(OtherProperty)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>($"data-val-{key}", Data.Error.FormatMessage(msg)));
        }

        protected override ValidationResult? IsValid(object? value, [NotNull] ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo is null)
            {
                var unknownProperty = GlobalResource.Validation_Compare_UnknownProperty;
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, unknownProperty, OtherProperty));
            }

            ValidationResult? result = null;

            if (value is IComparable firstComparable && otherPropertyInfo.GetValue(validationContext.ObjectInstance, null) is IComparable otherComparable)
            {
                var compareValue = firstComparable.CompareTo(otherComparable);

                OtherPropertyDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);

                var error = FormatErrorMessage(Globals.GetLocalizedDisplayName(validationContext.ObjectType.GetProperty(validationContext.MemberName!)), OtherPropertyDisplayName);
                switch (OperandType)
                {
                    case Constants.OperandType.GreaterThan:
                        if (compareValue <= 0)
                        {
                            result = new ValidationResult(error);
                        }

                        break;
                    case Constants.OperandType.GreaterThanOrEqual:
                        if (compareValue < 0)
                        {
                            result = new ValidationResult(error);
                        }

                        break;
                    case Constants.OperandType.LessThan:
                        if (compareValue >= 0)
                        {
                            result = new ValidationResult(error);
                        }

                        break;
                    case Constants.OperandType.LessThanOrEqual:
                        if (compareValue > 0)
                        {
                            result = new ValidationResult(error);
                        }

                        break;
                    case Constants.OperandType.Equals:
                        if (compareValue != 0)
                        {
                            result = new ValidationResult(error);
                        }

                        break;
                    case Constants.OperandType.NotEquals:
                        if (compareValue == 0)
                        {
                            result = new ValidationResult(error);
                        }

                        break;
                }
            }

            return result;
        }

        private string FormatErrorMessage(string? modelDisplayName, string? otherDisplayName) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, modelDisplayName, otherDisplayName ?? OtherPropertyDisplayName ?? OtherProperty, EnumHelper.LocalizeEnum(OperandType));
    }
}

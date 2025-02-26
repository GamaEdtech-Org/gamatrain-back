namespace GamaEdtech.Common.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DataTypeAttribute : System.ComponentModel.DataAnnotations.DataTypeAttribute
    {
        private DisplayFormatAttribute? displayFormat;

        public DataTypeAttribute(ElementDataType elementDataType)
            : base(elementDataType.ToString())
        {
            ElementDataType = elementDataType;

            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_DataType);

            switch (elementDataType)
            {
                case ElementDataType.Date:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:d}",
                        ApplyFormatInEditMode = true,
                    };
                    break;
                case ElementDataType.Time:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:t}",
                        ApplyFormatInEditMode = true,
                    };
                    break;
                case ElementDataType.Currency:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:C}",
                    };
                    break;
            }
        }

        public ElementDataType ElementDataType { get; }

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

        public new DisplayFormatAttribute? DisplayFormat
        {
            get => displayFormat;

            private set => base.DisplayFormat = displayFormat = value;
        }

        public new string? ErrorMessage => base.ErrorMessage;

        public override string GetDataTypeName() => ElementDataType.ToString();
    }
}

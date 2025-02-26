namespace GamaEdtech.Common.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ValidateInputAttribute : Attribute
    {
        public bool Markup { get; set; }

        public bool MultiLine { get; set; }

        public bool Scripting { get; set; }

        public bool SQL { get; set; }

        public bool AngleBrackets { get; set; }
    }
}

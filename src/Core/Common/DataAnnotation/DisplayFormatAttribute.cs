namespace GamaEdtech.Common.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field)]
    public sealed class DisplayFormatAttribute : System.ComponentModel.DataAnnotations.DisplayFormatAttribute
    {
        public Core.Constants.FormatProvider FormatProvider { get; set; }
    }
}

namespace GamaEdtech.Common.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        public DisplayNameAttribute()
            : base()
        {
        }

        public DisplayNameAttribute(string? displayName)
            : base(displayName ?? string.Empty)
        {
        }
    }
}

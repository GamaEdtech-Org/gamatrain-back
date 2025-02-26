namespace GamaEdtech.Common.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SuggestionsUrlAttribute(string url) : Attribute
    {
        public string? Url { get; } = url;
    }
}

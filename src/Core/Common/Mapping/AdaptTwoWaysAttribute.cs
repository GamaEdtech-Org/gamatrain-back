namespace GamaEdtech.Common.Mapping
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AdaptTwoWaysAttribute : Mapster.AdaptTwoWaysAttribute
    {
        public AdaptTwoWaysAttribute(Type type)
            : base(type)
        {
        }

        public AdaptTwoWaysAttribute(string name)
            : base(name)
        {
        }
    }
}

namespace GamaEdtech.Common.Localization
{
    using System.Collections.Generic;

    public static class CultureExtensions
    {
        public static IEnumerable<string> AtomicValues
        {
            get
            {
                yield return nameof(Culture.En);
                yield return nameof(Culture.Fa);
            }
        }
    }
}

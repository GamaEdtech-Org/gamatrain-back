namespace GamaEdtech.Common.Core.Extensions.Collections.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static class ListExtensions
    {
        public static bool Exists<T>([NotNull] this IReadOnlyList<T> list, Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(match);

            for (var i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

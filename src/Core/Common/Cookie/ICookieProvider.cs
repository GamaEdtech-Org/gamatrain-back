namespace GamaEdtech.Common.Cookie
{
    using System;
    using System.Threading.Tasks;

    using GamaEdtech.Common.DataAnnotation;

    [Injectable]
    public interface ICookieProvider
    {
        bool TryGetValue<TItem, TKey>(TKey key, out TItem? value)
            where TKey : struct;

        bool TryGetValue<TItem>(string? key, out TItem? value);

        Task<TItem?> GetAsync<TItem, TKey>(TKey key)
            where TKey : struct;

        Task<TItem?> GetAsync<TItem>(string? key);

        TItem? Get<TItem, TKey>(TKey key)
            where TKey : struct;

        TItem? Get<TItem>(string? key);

        void Remove<TKey>(TKey key)
            where TKey : struct;

        void Remove(string? key);

        Task<TItem?> SetAsync<TItem, TKey>(TKey key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? expires = null, bool httpOnly = true)
            where TKey : struct;

        Task<TItem?> SetAsync<TItem>(string? key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? expires = null, bool httpOnly = true);

        TItem? Set<TItem, TKey>(TKey key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? expires = null, bool httpOnly = true)
            where TKey : struct;

        TItem? Set<TItem>(string? key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? expires = null, bool httpOnly = true);
    }
}

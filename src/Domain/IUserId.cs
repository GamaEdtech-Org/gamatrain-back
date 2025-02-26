namespace GamaEdtech.Domain
{
    public interface IUserId<TKey>
    where TKey : IEquatable<TKey>
    {
        TKey UserId { get; set; }
    }
}

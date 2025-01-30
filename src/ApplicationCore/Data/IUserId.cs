namespace GamaEdtech.Backend.Data
{
    public interface IUserId<TKey>
    where TKey : IEquatable<TKey>
    {
        TKey UserId { get; set; }
    }
}

namespace GamaEdtech.Data.Dto.Game
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class CoinDto
    {
        public required Guid Id { get; set; }
        public required CoinType CoinType { get; set; }
        public required DateTimeOffset ExpirationTime { get; set; }
    }
}

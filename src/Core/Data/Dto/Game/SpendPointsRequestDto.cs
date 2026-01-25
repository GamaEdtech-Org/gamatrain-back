namespace GamaEdtech.Data.Dto.Game
{
    public sealed class SpendPointsRequestDto
    {
        public required int UserId { get; set; }
        public required long Points { get; set; }
    }
}

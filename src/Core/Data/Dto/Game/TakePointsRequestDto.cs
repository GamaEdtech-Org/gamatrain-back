namespace GamaEdtech.Data.Dto.Game
{
    public sealed class TakePointsRequestDto
    {
        public required int UserId { get; set; }
        public required int Points { get; set; }
    }
}

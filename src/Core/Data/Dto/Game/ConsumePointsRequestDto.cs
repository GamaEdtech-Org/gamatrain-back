namespace GamaEdtech.Data.Dto.Game
{
    public sealed class ConsumePointsRequestDto
    {
        public required int UserId { get; set; }
        public required int Points { get; set; }
    }
}

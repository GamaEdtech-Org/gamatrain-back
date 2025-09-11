namespace GamaEdtech.Data.Dto.Payment
{
    public sealed class ConvertRequestDto
    {
        public required decimal Amount { get; set; }
        public required string? SourceMint { get; set; }
        public required string? DestinationMint { get; set; }
    }
}

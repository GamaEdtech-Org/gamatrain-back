namespace GamaEdtech.Data.Dto.Transaction
{
    public sealed class GetStatisticsResponseDto
    {
        public string Name { get; set; }
        public long DebitValue { get; set; }
        public long CreditValue { get; set; }
    }
}

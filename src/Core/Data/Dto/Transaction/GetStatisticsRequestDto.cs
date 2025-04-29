namespace GamaEdtech.Data.Dto.Transaction
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class GetStatisticsRequestDto
    {
        public int UserId { get; set; }
        public Period Period { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}

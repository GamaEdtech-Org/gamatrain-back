namespace GamaEdtech.Data.Dto.Game
{
    public sealed class ExamResultResponseDto
    {
        public int Total { get; set; }
        public int Valid { get; set; }
        public int Invalid { get; set; }
        public int NoAnswer { get; set; }
        public double Percent { get; set; }
    }
}

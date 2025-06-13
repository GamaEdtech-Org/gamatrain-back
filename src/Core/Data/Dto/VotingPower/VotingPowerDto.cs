namespace GamaEdtech.Data.Dto.VotingPower
{
    public sealed class VotingPowerDto
    {
        public long Id { get; set; }
        public string? ProposalId { get; set; }
        public string? WalletAddress { get; set; }
        public decimal? Amount { get; set; }
        public string? TokenAccount { get; set; }
    }
}

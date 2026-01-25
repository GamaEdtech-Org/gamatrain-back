namespace GamaEdtech.Presentation.ViewModel.VotingPower
{
    public sealed class VotingPowersResponseViewModel
    {
        public long Id { get; set; }

        public string? ProposalId { get; set; }

        public string? WalletAddress { get; set; }

        public decimal? Amount { get; set; }

        public string? TokenAccount { get; set; }
    }
}

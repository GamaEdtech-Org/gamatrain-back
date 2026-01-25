namespace GamaEdtech.Data.Dto.VotingPower
{
    using System;

    public sealed class ManageVotingPowerRequestDto
    {
        public string? ProposalId { get; set; }
        public string? WalletAddress { get; set; }
        public decimal? Amount { get; set; }
        public string? TokenAccount { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}

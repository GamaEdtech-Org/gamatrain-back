namespace GamaEdtech.Presentation.ViewModel.VotingPower
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ManageVotingPowerRequestViewModel
    {
        [Display]
        [Required]
        public string? ProposalId { get; set; }

        [Display]
        [Required]
        public string? WalletAddress { get; set; }

        [Display]
        [Required]
        public decimal? Amount { get; set; }

        [Display]
        [Required]
        public string? TokenAccount { get; set; }
    }
}

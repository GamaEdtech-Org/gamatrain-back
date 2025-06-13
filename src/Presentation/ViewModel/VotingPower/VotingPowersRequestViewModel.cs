namespace GamaEdtech.Presentation.ViewModel.VotingPower
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class VotingPowersRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public string? ProposalId { get; set; }

        [Display]
        public string? WalletAddress { get; set; }
    }
}

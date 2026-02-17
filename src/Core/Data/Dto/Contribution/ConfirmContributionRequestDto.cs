namespace GamaEdtech.Data.Dto.Contribution
{
    using GamaEdtech.Common.DataAccess.Specification;

    public sealed class ConfirmContributionRequestDto<T>
    {
        public required ISpecification<T> Specification { get; set; }
        public required bool NotifyUser { get; set; }
    }
}

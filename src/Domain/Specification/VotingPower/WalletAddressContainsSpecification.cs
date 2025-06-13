namespace GamaEdtech.Domain.Specification.VotingPower
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class WalletAddressContainsSpecification(string walletAddress) : SpecificationBase<VotingPower>
    {
        public override Expression<Func<VotingPower, bool>> Expression() => (t) => t.WalletAddress!.Contains(walletAddress);
    }
}

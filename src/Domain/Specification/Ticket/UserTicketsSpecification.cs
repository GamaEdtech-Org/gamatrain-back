namespace GamaEdtech.Domain.Specification.Ticket
{
    using System.Linq.Expressions;
    using System.Security.Claims;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class UserTicketsSpecification(ClaimsPrincipal user) : SpecificationBase<Ticket>
    {
        public override Expression<Func<Ticket, bool>> Expression()
        {
            var userId = user.UserId();
            var email = user.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Email)?.Value.ToLowerInvariant();
            return (t) => userId.Equals(t.UserId) || t.Email == email;
        }
    }
}

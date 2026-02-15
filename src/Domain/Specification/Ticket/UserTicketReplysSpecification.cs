namespace GamaEdtech.Domain.Specification.Ticket
{
    using System.Linq.Expressions;
    using System.Security.Claims;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class UserTicketReplysSpecification(long ticketId, ClaimsPrincipal user) : SpecificationBase<TicketReply>
    {
        public override Expression<Func<TicketReply, bool>> Expression()
        {
            var userId = user.UserId();
            var email = user.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Email)?.Value.ToLowerInvariant();
            return (t) => t.TicketId == ticketId && (userId.Equals(t.Ticket.UserId) || t.Ticket.Email == email);
        }
    }
}

namespace GamaEdtech.Domain.Specification.Ticket
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class TicketIdEqualsSpecification(long ticketId) : SpecificationBase<TicketReply>
    {
        public override Expression<Func<TicketReply, bool>> Expression() => (t) => t.TicketId.Equals(ticketId);
    }
}

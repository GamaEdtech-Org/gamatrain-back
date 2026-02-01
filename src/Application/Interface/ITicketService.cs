namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Ticket;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface ITicketService
    {
        Task<ResultData<ListDataSource<TicketsDto>>> GetTicketsAsync(ListRequestDto<Ticket>? requestDto = null);
        Task<ResultData<TicketDto>> GetTicketAsync([NotNull] ISpecification<Ticket> specification);
        Task<ResultData<long>> CreateTicketAsync([NotNull] CreateTicketRequestDto requestDto);
        Task<ResultData<bool>> ToggleIsReadAsync([NotNull] ISpecification<Ticket> specification);
        Task<ResultData<bool>> RemoveTicketAsync([NotNull] ISpecification<Ticket> specification);
    }
}

namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Ticket;

    using Microsoft.AspNetCore.Mvc;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class TicketsController(Lazy<ILogger<TicketsController>> logger, Lazy<ITicketService> ticketService)
        : ApiControllerBase<TicketsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<TicketsResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<TicketsResponseViewModel>>> GetTickets([NotNull, FromQuery] TicketsRequestViewModel request)
        {
            try
            {
                var result = await ticketService.Value.GetTicketsAsync(new ListRequestDto<Ticket>
                {
                    PagingDto = request.PagingDto,
                });
                return Ok<ListDataSource<TicketsResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new TicketsResponseViewModel
                        {
                            Id = t.Id,
                            Subject = t.Subject,
                            FullName = t.Sender,
                            Email = t.Email,
                            IsRead = t.IsRead,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ListDataSource<TicketsResponseViewModel>>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpGet("{id:long}"), Produces<ApiResponse<TicketResponseViewModel>>()]
        public async Task<IActionResult<TicketResponseViewModel>> GetTicket([FromRoute] long id)
        {
            try
            {
                var result = await ticketService.Value.GetTicketAsync(new IdEqualsSpecification<Ticket, long>(id));
                return Ok<TicketResponseViewModel>(new(result.Errors)
                {
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Body = result.Data.Body,
                        Email = result.Data.Email,
                        FullName = result.Data.Sender,
                        Subject = result.Data.Subject,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<TicketResponseViewModel>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPatch("{id:long}/toggle"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult<bool>> ToggleIsRead([FromRoute] long id)
        {
            try
            {
                var result = await ticketService.Value.ToggleIsReadAsync(new IdEqualsSpecification<Ticket, long>(id));
                return Ok<bool>(new(result.Errors)
                {
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("{id:long}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult<bool>> RemoveTicket([FromRoute] long id)
        {
            try
            {
                var result = await ticketService.Value.RemoveTicketAsync(new IdEqualsSpecification<Ticket, long>(id));
                return Ok<bool>(new(result.Errors)
                {
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}

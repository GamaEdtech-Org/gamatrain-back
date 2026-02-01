namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Linq;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.Ticket;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class TicketService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<TicketService>> localizer
        , Lazy<ILogger<TicketService>> logger, Lazy<IFileService> fileService)
        : LocalizableServiceBase<TicketService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), ITicketService
    {
        public async Task<ResultData<ListDataSource<TicketsDto>>> GetTicketsAsync(ListRequestDto<Ticket>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<Ticket>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new TicketsDto
                {
                    Id = t.Id,
                    Email = t.Email,
                    Sender = t.Sender,
                    IsRead = t.IsRead,
                    IsReadByAdmin = t.IsReadByAdmin,
                    Subject = t.Subject,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<TicketDto>> GetTicketAsync([NotNull] ISpecification<Ticket> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Ticket>();
                var ticket = await repository.GetManyQueryable(specification).Select(t => new
                {
                    t.Id,
                    t.Body,
                    t.Subject,
                    t.Sender,
                    t.Email,
                    t.CreationDate,
                    t.ParentId,
                    t.FileId,
                }).FirstOrDefaultAsync();
                if (ticket is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["TicketNotFound"] },],
                    };
                }

                TicketDto result = new()
                {
                    Id = ticket.Id,
                    Subject = ticket.Subject,
                    Body = ticket.Body,
                    Sender = ticket.Sender,
                    Email = ticket.Email,
                    CreationDate = ticket.CreationDate,
                    ParentId = ticket.ParentId,
                    FileUri = fileService.Value.GetFileUri(ticket.FileId, ContainerType.Ticket).Data,
                };

                _ = await repository.GetManyQueryable(specification).ExecuteUpdateAsync(t => t.SetProperty(p => p.IsRead, true));

                return new(OperationResult.Succeeded) { Data = result };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<long>> CreateTicketAsync([NotNull] CreateTicketRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Ticket>();

                if (requestDto.ParentId.HasValue)
                {
                    var exists = await repository.AnyAsync(t => t.Id == requestDto.ParentId.Value);
                    if (!exists)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["TicketNotFound"] },],
                        };
                    }
                }

                var (fileId, errors) = await SaveFileAsync(requestDto.File);
                if (errors is not null)
                {
                    return new(OperationResult.Failed)
                    {
                        Errors = errors,
                    };
                }

                var ticket = new Ticket
                {
                    Sender = requestDto.Sender,
                    Body = requestDto.Body,
                    Email = requestDto.Email,
                    Subject = requestDto.Subject,
                    CreationDate = DateTimeOffset.UtcNow,
                    IsRead = true,
                    IsReadByAdmin = false,
                    CreationUserId = requestDto.CreationUserId,
                    ParentId = requestDto.ParentId,
                    FileId = fileId,
                };
                repository.Add(ticket);
                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = ticket.Id };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> ToggleIsReadAsync([NotNull] ISpecification<Ticket> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var rowAffected = await uow.GetRepository<Ticket>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.IsRead, p => !p.IsRead));

                return rowAffected == 0
                    ? new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["TicketNotFound"] },],
                    }
                    : new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> RemoveTicketAsync([NotNull] ISpecification<Ticket> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var rowAffected = await uow.GetRepository<Ticket>().GetManyQueryable(specification)
                    .ExecuteDeleteAsync();

                return rowAffected == 0
                    ? new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["TicketNotFound"] },],
                    }
                    : new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        private async Task<(string? FileId, IEnumerable<Error>? Errors)> SaveFileAsync(IFormFile? file)
        {
            if (file is null)
            {
                return (null, null);
            }

            using MemoryStream stream = new();
            await file.CopyToAsync(stream);

            var fileId = await fileService.Value.UploadFileAsync(new()
            {
                File = stream.ToArray(),
                ContainerType = ContainerType.Ticket,
                FileExtension = Path.GetExtension(file.FileName),
            });

            return fileId.OperationResult is OperationResult.Succeeded
                ? ((string? ImageId, IEnumerable<Error>? Errors))(fileId.Data, null)
                : new(null, fileId.Errors);
        }
    }
}

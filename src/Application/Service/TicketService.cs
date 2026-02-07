namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.RegularExpressions;

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

    public partial class TicketService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<TicketService>> localizer
        , Lazy<ILogger<TicketService>> logger, Lazy<IFileService> fileService, Lazy<IEmailService> emailService)
        : LocalizableServiceBase<TicketService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), ITicketService
    {
        public async Task<ResultData<ListDataSource<TicketsDto>>> GetTicketsAsync(ListRequestDto<Ticket>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<Ticket>().GetManyQueryable(requestDto?.Specification)
                    .OrderBy(t => t.IsReadByAdmin).ThenBy(t => t.TicketReplys.Any(r => !r.IsReadByAdmin)).ThenByDescending(t => t.CreationDate).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new TicketsDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    Email = t.Email,
                    IsReadByAdmin = t.IsReadByAdmin,
                    Subject = t.Subject,
                    CreationDate = t.CreationDate,
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
                    t.FullName,
                    CreationUser = t.CreationUser == null ? null : (t.CreationUser.FirstName + " " + t.CreationUser.LastName),
                    t.CreationDate,
                    t.Email,
                    t.Subject,
                    t.Body,
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
                    FullName = ticket.FullName,
                    CreationUser = ticket.CreationUser,
                    CreationDate = ticket.CreationDate,
                    Email = ticket.Email,
                    Subject = ticket.Subject,
                    Body = ticket.Body,
                    FileUri = fileService.Value.GetFileUri(ticket.FileId, ContainerType.Ticket).Data,
                };

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
                    FullName = requestDto.FullName,
                    Body = requestDto.Body,
                    Email = requestDto.Email,
                    Subject = requestDto.Subject,
                    CreationDate = DateTimeOffset.UtcNow,
                    IsReadByAdmin = false,
                    CreationUserId = requestDto.CreationUserId,
                    FileId = fileId,
                };
                repository.Add(ticket);
                _ = await uow.SaveChangesAsync();

                _ = await emailService.Value.SendEmailAsync(new()
                {
                    Subject = GenerateSubject(ticket.Id, "Your support request"),
                    Body = $"Hi {requestDto.FullName},<br><br>We received your support request<br><br>{requestDto.Subject}<br><br>{requestDto.Body}",
                    EmailAddresses = [requestDto.Email!],
                    SenderName = "Gamatrain",
                });

                return new(OperationResult.Succeeded) { Data = ticket.Id };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<IEnumerable<TicketReplyDto>>> GetTicketReplysAsync([NotNull] ISpecification<TicketReply> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var lst = await uow.GetRepository<TicketReply>().GetManyQueryable(specification).Select(t => new
                {
                    t.Id,
                    t.CreationDate,
                    t.Body,
                    CreationUser = t.CreationUser == null ? null : t.CreationUser.FirstName + " " + t.CreationUser.LastName,
                    t.FileId,
                }).ToListAsync();

                var result = lst.Select(t => new TicketReplyDto
                {
                    Id = t.Id,
                    Body = t.Body,
                    CreationDate = t.CreationDate,
                    CreationUser = t.CreationUser,
                    FileUri = fileService.Value.GetFileUri(t.FileId, ContainerType.Ticket).Data,
                });
                return new(OperationResult.Succeeded) { Data = result };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed)
                {
                    Errors = [new() { Message = exc.Message, }],
                };
            }
        }

        public async Task<ResultData<long>> ReplyTicketAsync([NotNull] ReplyTicketRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<TicketReply>();

                var (fileId, errors) = await SaveFileAsync(requestDto.File);
                if (errors is not null)
                {
                    return new(OperationResult.Failed)
                    {
                        Errors = errors,
                    };
                }

                var reply = new TicketReply
                {
                    TicketId = requestDto.TicketId,
                    Body = requestDto.Body,
                    CreationDate = DateTimeOffset.UtcNow,
                    IsRead = !requestDto.ReplyByAdmin,
                    IsReadByAdmin = !requestDto.ReplyByAdmin,
                    CreationUserId = requestDto.CreationUserId,
                    FileId = fileId,
                };
                repository.Add(reply);
                _ = await uow.SaveChangesAsync();

                if (requestDto.ReplyByAdmin)
                {
                    var data = await uow.GetRepository<Ticket>().GetManyQueryable(t => t.Id == requestDto.TicketId).Select(t => new
                    {
                        t.Email,
                        t.Subject,
                    }).FirstOrDefaultAsync();

                    _ = await emailService.Value.SendEmailAsync(new()
                    {
                        Body = requestDto.Body,
                        Subject = GenerateSubject(requestDto.TicketId, data!.Subject),
                        EmailAddresses = [data.Email!],
                        SenderName = requestDto.SenderName ?? "Gamatrain",
                    });
                }

                return new(OperationResult.Succeeded) { Data = reply.Id };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> SetReplysAsReadedByAdminAsync([NotNull] ISpecification<TicketReply> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var rowAffected = await uow.GetRepository<TicketReply>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.IsReadByAdmin, true));

                return rowAffected == 0
                    ? new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["TicketReplyNotFound"] },],
                    }
                    : new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> ToggleIsReadByAdminAsync([NotNull] ISpecification<Ticket> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var rowAffected = await uow.GetRepository<Ticket>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.IsReadByAdmin, p => !p.IsReadByAdmin));

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
                var repository = uow.GetRepository<Ticket>();
                var ticket = await repository.GetAsync(specification);
                if (ticket is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["TicketNotFound"] },],
                    };
                }

                repository.Remove(ticket);
                _ = uow.SaveChangesAsync();

                _ = await fileService.Value.RemoveFileAsync(new()
                {
                    ContainerType = ContainerType.Ticket,
                    FileId = ticket.FileId,
                });

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task ProccessInboundEmailAsync(HttpRequest request)
        {
            Logger.Value.LogException(new Exception("ProccessInboundEmailAsync 1"));
            var result = await emailService.Value.ProccessInboundEmailAsync(request);
            Logger.Value.LogException(new Exception("ProccessInboundEmailAsync 2"));
            if (result.Data is null)
            {
                return;
            }

            Logger.Value.LogException(new Exception($"ProccessInboundEmailAsync 3: {JsonSerializer.Serialize(result.Data)}"));
            var match = TicketRegex().Match(result.Data.Subject!);
            if (match.Success)
            {
                Logger.Value.LogException(new Exception("ProccessInboundEmailAsync 4"));
                var ticketId = match.Groups[0].Value.ValueOf<long?>();
                Logger.Value.LogException(new Exception($"ProccessInboundEmailAsync 5: {ticketId}"));
                if (ticketId.HasValue)
                {
                    Logger.Value.LogException(new Exception($"ProccessInboundEmailAsync 6"));
                    var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                    var email = await uow.GetRepository<Ticket>().GetManyQueryable(t => t.Id == ticketId).Select(t => t.Email).FirstOrDefaultAsync();
                    if (result.Data.From!.Contains(email!, StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.Value.LogException(new Exception($"ProccessInboundEmailAsync 7"));
                        _ = await ReplyTicketAsync(new()
                        {
                            Body = result.Data.Body!,
                            TicketId = ticketId.Value,
                            ReplyByAdmin = false,
                        });
                        Logger.Value.LogException(new Exception($"ProccessInboundEmailAsync 8"));
                        return;
                    }
                }
            }

            Logger.Value.LogException(new Exception($"ProccessInboundEmailAsync 9"));
            _ = await CreateTicketAsync(new()
            {
                Body = result.Data.Body,
                Subject = result.Data.Subject,
                Email = result.Data.From,
                FullName = "Customer",
            });
            Logger.Value.LogException(new Exception($"ProccessInboundEmailAsync 10"));
        }

        private static string GenerateSubject(long ticketId, string? subject) => $"Reply [Ticket-{ticketId}] {subject}";

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

        [GeneratedRegex("\\[Ticket-(\\d*)]")]
        private static partial Regex TicketRegex();
    }
}

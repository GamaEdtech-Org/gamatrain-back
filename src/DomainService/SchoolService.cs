namespace GamaEdtech.Backend.DomainService
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using EntityFramework.Exceptions.Common;

    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Linq;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using Farsica.Framework.Service;

    using GamaEdtech.Backend.Data.Dto.School;
    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static Farsica.Framework.Core.Constants;

    public class SchoolService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<FileService>> localizer
        , Lazy<ILogger<FileService>> logger)
        : LocalizableServiceBase<FileService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), ISchoolService
    {
        public async Task<ResultData<ListDataSource<SchoolsDto>>> GetSchoolsAsync(ListRequestDto<School>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<School, int>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new SchoolsDto
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<SchoolDto>> GetSchoolAsync([NotNull] ISpecification<School> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var school = await uow.GetRepository<School, int>().GetManyQueryable(specification).Select(t => new SchoolDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Address = t.Address,
                    Latitude = t.Location!.X,
                    Longitude = t.Location!.Y,
                    SchoolType = t.SchoolType!,
                    StateId = t.StateId,
                    StateTitle = t.State!.Title,
                    ZipCode = t.ZipCode,
                }).FirstOrDefaultAsync();

                return school is null
                    ? new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["SchoolNotFound"] },],
                    }
                    : new(OperationResult.Succeeded) { Data = school };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<int>> ManageSchoolAsync([NotNull] ManageSchoolRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<School, int>();
                School? school = null;

                if (requestDto.Id.HasValue)
                {
                    school = await repository.GetAsync(requestDto.Id.Value);
                    if (school is null)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["SchoolNotFound"] },],
                        };
                    }

                    school.Name = requestDto.Name;
                    school.Address = requestDto.Address;
                    school.Location = new NetTopologySuite.Geometries.Point(requestDto.Latitude, requestDto.Longitude);
                    school.SchoolType = requestDto.SchoolType;
                    school.StateId = requestDto.StateId;
                    school.ZipCode = requestDto.ZipCode;

                    _ = repository.Update(school);
                }
                else
                {
                    school = new School
                    {
                        Name = requestDto.Name,
                        Address = requestDto.Address,
                        Location = new NetTopologySuite.Geometries.Point(requestDto.Latitude, requestDto.Longitude),
                        SchoolType = requestDto.SchoolType,
                        StateId = requestDto.StateId,
                        ZipCode = requestDto.ZipCode,
                    };
                    repository.Add(school);
                }

                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = school.Id };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["InvalidStateId"], }] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> RemoveSchoolAsync([NotNull] ISpecification<School> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var school = await uow.GetRepository<School, int>().GetAsync(specification);
                if (school is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Data = false,
                        Errors = [new() { Message = Localizer.Value["SchoolNotFound"] },],
                    };
                }

                uow.GetRepository<School, int>().Remove(school);
                _ = await uow.SaveChangesAsync();
                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["SchoolCantBeRemoved"], },] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }
    }
}

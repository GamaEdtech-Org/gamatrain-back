namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using EntityFramework.Exceptions.Common;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Linq;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class SchoolService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<FileService>> localizer
        , Lazy<ILogger<FileService>> logger, Lazy<IFileService> fileService)
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
                    LocalName = t.LocalName,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<ListDataSource<SchoolInfoDto>>> GetSchoolsListAsync(ListRequestDto<School>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<School, int>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new SchoolInfoDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    LastModifyDate = t.LastModifyDate ?? t.CreationDate,
                    HasWebSite = string.IsNullOrEmpty(t.WebSite),
                    HasEmail = string.IsNullOrEmpty(t.Email),
                    HasPhoneNumber = string.IsNullOrEmpty(t.PhoneNumber),
                    Location = t.Location,
                    Score = t.Comments != null ? t.Comments.Average(c => c.AverageRate) : null,
                    CityTitle = t.City == null ? "" : t.City.Title,
                    CountryTitle = t.Country == null ? "" : t.Country.Title,
                    StateTitle = t.State == null ? "" : t.State.Title,
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
                    LocalName = t.LocalName,
                    Address = t.Address,
                    LocalAddress = t.LocalAddress,
                    Location = t.Location,
                    SchoolType = t.SchoolType,
                    ZipCode = t.ZipCode,
                    CityId = t.CityId,
                    CityTitle = t.City != null ? t.City.Title : "",
                    CountryId = t.CountryId,
                    CountryTitle = t.Country != null ? t.Country.Title : "",
                    StateId = t.StateId,
                    StateTitle = t.State != null ? t.State.Title : "",
                    Facilities = t.Facilities,
                    WebSite = t.WebSite,
                    FaxNumber = t.FaxNumber,
                    PhoneNumber = t.PhoneNumber,
                    Email = t.Email,
                    Quarter = t.Quarter,
                    OsmId = t.OsmId,
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
                    school.LocalName = requestDto.LocalName;
                    school.Address = requestDto.Address;
                    school.Location = requestDto.Location;
                    school.SchoolType = requestDto.SchoolType;
                    school.StateId = requestDto.StateId;
                    school.ZipCode = requestDto.ZipCode;
                    school.WebSite = requestDto.WebSite;
                    school.Quarter = requestDto.Quarter;
                    school.PhoneNumber = requestDto.PhoneNumber;
                    school.LocalAddress = requestDto.LocalAddress;
                    school.FaxNumber = requestDto.FaxNumber;
                    school.Facilities = requestDto.Facilities;
                    school.Email = requestDto.Email;
                    school.CityId = requestDto.CityId;
                    school.CountryId = requestDto.CountryId;
                    school.OsmId = requestDto.OsmId;

                    _ = repository.Update(school);
                }
                else
                {
                    school = new School
                    {
                        Name = requestDto.Name,
                        LocalName = requestDto.LocalName,
                        Address = requestDto.Address,
                        Location = requestDto.Location,
                        SchoolType = requestDto.SchoolType,
                        StateId = requestDto.StateId,
                        ZipCode = requestDto.ZipCode,
                        WebSite = requestDto.WebSite,
                        Quarter = requestDto.Quarter,
                        PhoneNumber = requestDto.PhoneNumber,
                        LocalAddress = requestDto.LocalAddress,
                        FaxNumber = requestDto.FaxNumber,
                        Facilities = requestDto.Facilities,
                        Email = requestDto.Email,
                        CityId = requestDto.CityId,
                        CountryId = requestDto.CountryId,
                        OsmId = requestDto.OsmId,
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

        public async Task<ResultData<SchoolRateDto>> GetSchoolRateAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .GroupBy(t => t.SchoolId)
                    .Select(t => new SchoolRateDto
                    {
                        AverageRate = t.Average(c => c.AverageRate),
                        TuitionRatioRate = t.Average(c => c.TuitionRatioRate),
                        SafetyAndHappinessRate = t.Average(c => c.SafetyAndHappinessRate),
                        ITTrainingRate = t.Average(c => c.ITTrainingRate),
                        FacilitiesRate = t.Average(c => c.FacilitiesRate),
                        EducationRate = t.Average(c => c.EducationRate),
                        ClassesQualityRate = t.Average(c => c.ClassesQualityRate),
                        BehaviorRate = t.Average(c => c.BehaviorRate),
                        ArtisticActivitiesRate = t.Average(c => c.ArtisticActivitiesRate),
                    }).FirstOrDefaultAsync();

                return new(OperationResult.Succeeded) { Data = result };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<ListDataSource<SchoolCommentDto>>> GetSchoolCommentsAsync(ListRequestDto<SchoolComment>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolComment>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new SchoolCommentDto
                {
                    Id = t.Id,
                    Comment = t.Comment,
                    CreationUser = t.CreationUser!.FullName,
                    CreationUserAvatar = t.CreationUser!.Avatar,
                    CreationDate = t.CreationDate,
                    LikeCount = t.LikeCount,
                    DislikeCount = t.DislikeCount,
                    AverageRate = t.AverageRate,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<long>> ManageSchoolCommentAsync([NotNull] ManageSchoolCommentRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<SchoolComment>();
                SchoolComment? schoolComment = null;

                if (requestDto.Id.HasValue)
                {
                    schoolComment = await repository.GetAsync(requestDto.Id.Value);
                    if (schoolComment is null)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["SchoolCommentNotFound"] },],
                        };
                    }

                    if (schoolComment.CreationUserId != requestDto.CreationUserId)
                    {
                        return new(OperationResult.NotValid)
                        {
                            Errors = [new() { Message = Localizer.Value["InvalidRequest"] },],
                        };
                    }

                    schoolComment.Comment = requestDto.Comment;
                    schoolComment.CreationDate = requestDto.CreationDate;
                    schoolComment.CreationUserId = requestDto.CreationUserId;

                    schoolComment.ArtisticActivitiesRate = requestDto.ArtisticActivitiesRate;
                    schoolComment.BehaviorRate = requestDto.BehaviorRate;
                    schoolComment.ClassesQualityRate = requestDto.ClassesQualityRate;
                    schoolComment.EducationRate = requestDto.EducationRate;
                    schoolComment.FacilitiesRate = requestDto.FacilitiesRate;
                    schoolComment.ITTrainingRate = requestDto.ITTrainingRate;
                    schoolComment.SafetyAndHappinessRate = requestDto.SafetyAndHappinessRate;
                    schoolComment.TuitionRatioRate = requestDto.TuitionRatioRate;
                    schoolComment.AverageRate = Calculate(schoolComment);

                    _ = repository.Update(schoolComment);
                }
                else
                {
                    schoolComment = new SchoolComment
                    {
                        SchoolId = requestDto.SchoolId,
                        ArtisticActivitiesRate = requestDto.ArtisticActivitiesRate,
                        BehaviorRate = requestDto.BehaviorRate,
                        ClassesQualityRate = requestDto.ClassesQualityRate,
                        Comment = requestDto.Comment,
                        CreationDate = requestDto.CreationDate,
                        CreationUserId = requestDto.CreationUserId,
                        EducationRate = requestDto.EducationRate,
                        FacilitiesRate = requestDto.FacilitiesRate,
                        ITTrainingRate = requestDto.ITTrainingRate,
                        SafetyAndHappinessRate = requestDto.SafetyAndHappinessRate,
                        TuitionRatioRate = requestDto.TuitionRatioRate,
                        Status = Status.Draft,
                    };
                    schoolComment.AverageRate = Calculate(schoolComment);
                    repository.Add(schoolComment);
                }

                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = schoolComment.Id };

                static double Calculate(SchoolComment comment) => (double)(
                        comment.ArtisticActivitiesRate +
                        comment.BehaviorRate +
                        comment.ClassesQualityRate +
                        comment.EducationRate +
                        comment.FacilitiesRate +
                        comment.ITTrainingRate +
                        comment.SafetyAndHappinessRate +
                        comment.TuitionRatioRate
                        ) / 8;
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["InvalidData"], }] };
            }
            catch (UniqueConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["DuplicateData"], }] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> LikeSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.LikeCount, p => p.LikeCount + 1));

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> DislikeSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.DislikeCount, p => p.DislikeCount + 1));

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> ConfirmSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Confirmed)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> RejectSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Rejected)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<ListDataSource<SchoolImageDto>>> GetSchoolImagesAsync(ListRequestDto<SchoolImage>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolImage>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new SchoolImageDto
                {
                    Id = t.Id,
                    CreationUser = t.CreationUser!.FullName,
                    CreationUserAvatar = t.CreationUser!.Avatar,
                    CreationDate = t.CreationDate,
                    FileId = t.FileId,
                    FileType = t.FileType,
                    SchoolId = t.SchoolId,
                    SchoolName = t.School!.Name,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<IEnumerable<string?>>> GetSchoolImagesPathAsync([NotNull] ISpecification<SchoolImage> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolImage>().GetManyQueryable(specification)
                    .Select(t => t.FileId).ToListAsync();
                return new(OperationResult.Succeeded) { Data = result.Select(t => fileService.Value.GetFileUri(t.ToString()).Data?.ToString()) };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<bool>> ConfirmSchoolImageAsync([NotNull] ISpecification<SchoolImage> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolImage>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Confirmed)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> RejectSchoolImageAsync([NotNull] ISpecification<SchoolImage> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolImage>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Rejected)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }
    }
}

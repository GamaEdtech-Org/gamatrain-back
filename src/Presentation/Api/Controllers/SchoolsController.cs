namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Domain.Specification.School;
    using GamaEdtech.Presentation.ViewModel.School;

    using Microsoft.AspNetCore.Mvc;

    using NetTopologySuite.Geometries;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SchoolsController(Lazy<ILogger<SchoolsController>> logger, Lazy<ISchoolService> schoolService)
        : ApiControllerBase<SchoolsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<SchoolInfoResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<SchoolInfoResponseViewModel>>> GetSchools([NotNull, FromQuery] SchoolInfoRequestViewModel request)
        {
            try
            {
                ISpecification<School>? baseSpecification = null;
                if (request.CountryId.HasValue)
                {
                    baseSpecification = new CountryIdEqualsSpecification(request.CountryId.Value);
                }
                if (request.StateId.HasValue)
                {
                    var specification = new StateIdEqualsSpecification(request.StateId.Value);
                    baseSpecification = baseSpecification is null ? specification : baseSpecification.And(specification);
                }
                if (request.CityId.HasValue)
                {
                    var specification = new CityIdEqualsSpecification(request.CityId.Value);
                    baseSpecification = baseSpecification is null ? specification : baseSpecification.And(specification);
                }
                if (!string.IsNullOrEmpty(request.Name))
                {
                    var specification = new NameContainsSpecification(request.Name);
                    baseSpecification = baseSpecification is null ? specification : baseSpecification.And(specification);
                }
                if (request.HasScore.HasValue)
                {
                    var specification = new HasScoreEqualsSpecification(request.HasScore.Value);
                    baseSpecification = baseSpecification is null ? specification : baseSpecification.And(specification);
                }

                Point? point = null;
                if (request.Location is not null)
                {
                    var specification = new LocationIncludeSpecification(request.Location.Latitude!.Value, request.Location.Longitude!.Value, request.Location.Radius!.Value);
                    point = specification.Point;
                    baseSpecification = baseSpecification is null ? specification : baseSpecification.And(specification);
                }

                request.PagingDto ??= new();
                request.PagingDto!.SortFilter = [new SortFilter { Column = "", SortType = Constants.SortType.Asc }];
                var result = await schoolService.Value.GetSchoolsListAsync(new ListRequestDto<School>
                {
                    PagingDto = request.PagingDto,
                    Specification = baseSpecification,
                }, point);
                return Ok(new ApiResponseWithFilter<ListDataSource<SchoolInfoResponseViewModel>>(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new SchoolInfoResponseViewModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Slug = t.Name.Slugify(),
                            LastModifyDate = t.LastModifyDate,
                            Score = t.Score,
                            CityTitle = t.CityTitle,
                            CountryTitle = t.CountryTitle,
                            HasEmail = t.HasEmail,
                            HasPhone = t.HasPhoneNumber,
                            HasWebsite = t.HasWebSite,
                            HasLocation = t.Coordinates is not null,
                            Latitude = t.Coordinates?.Y,
                            Longitude = t.Coordinates?.X,
                            StateTitle = t.StateTitle,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    },
                    Filters = [
                        new(JsonNamingPolicy.KebabCaseLower.ConvertName(nameof(SchoolInfoResponseViewModel.CityTitle)), request.CityId.HasValue ? result.Data.List?.FirstOrDefault()?.CityTitle : null),
                        new(JsonNamingPolicy.KebabCaseLower.ConvertName(nameof(SchoolInfoResponseViewModel.CountryTitle)), request.CountryId.HasValue ? result.Data.List?.FirstOrDefault()?.CountryTitle: null),
                        new(JsonNamingPolicy.KebabCaseLower.ConvertName(nameof(SchoolInfoResponseViewModel.StateTitle)), request.StateId.HasValue ? result.Data.List?.FirstOrDefault()?.StateTitle: null),
                    ]
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<SchoolInfoResponseViewModel>>(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("{id:int}"), Produces<ApiResponse<SchoolResponseViewModel>>()]
        public async Task<IActionResult<SchoolResponseViewModel>> GetSchool([FromRoute] int id)
        {
            try
            {
                var result = await schoolService.Value.GetSchoolAsync(new IdEqualsSpecification<School, int>(id));

                return Ok(new ApiResponse<SchoolResponseViewModel>(result.Errors)
                {
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Address = result.Data.Address,
                        LocalAddress = result.Data.LocalAddress,
                        Name = result.Data.Name,
                        LocalName = result.Data.LocalName,
                        SchoolType = result.Data.SchoolType,
                        StateId = result.Data.StateId,
                        StateTitle = result.Data.StateTitle,
                        ZipCode = result.Data.ZipCode,
                        Latitude = result.Data.Coordinates?.Y,
                        Longitude = result.Data.Coordinates?.X,
                        Facilities = result.Data.Facilities,
                        WebSite = result.Data.WebSite,
                        Email = result.Data.Email,
                        CityId = result.Data.CityId,
                        CityTitle = result.Data.CityTitle,
                        CountryId = result.Data.CountryId,
                        CountryTitle = result.Data.CountryTitle,
                        FaxNumber = result.Data.FaxNumber,
                        PhoneNumber = result.Data.PhoneNumber,
                        Quarter = result.Data.Quarter,
                        OsmId = result.Data.OsmId,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<SchoolResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        #region Comments

        [HttpGet("{schoolId:int}/rate"), Produces<ApiResponse<SchoolRateResponseViewModel>>()]
        public async Task<IActionResult<SchoolRateResponseViewModel>> GetSchoolRate([FromRoute] int schoolId)
        {
            try
            {
                var result = await schoolService.Value.GetSchoolRateAsync(new SchoolIdEqualsSpecification<SchoolComment>(schoolId));

                return Ok(new ApiResponse<SchoolRateResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = result.Data is null ? new() : new()
                    {
                        ArtisticActivitiesRate = result.Data.ArtisticActivitiesRate,
                        AverageRate = result.Data.AverageRate,
                        BehaviorRate = result.Data.BehaviorRate,
                        ClassesQualityRate = result.Data.ClassesQualityRate,
                        EducationRate = result.Data.EducationRate,
                        FacilitiesRate = result.Data.FacilitiesRate,
                        ITTrainingRate = result.Data.ITTrainingRate,
                        SafetyAndHappinessRate = result.Data.SafetyAndHappinessRate,
                        TuitionRatioRate = result.Data.TuitionRatioRate,
                        TotalCount = result.Data.TotalCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<SchoolRateResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("{schoolId:int}/comments"), Produces<ApiResponse<ListDataSource<SchoolCommentsResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<SchoolCommentsResponseViewModel>>> GetSchoolComments([FromRoute] int schoolId, [NotNull, FromQuery] SchoolCommentsRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.GetSchoolCommentsAsync(new ListRequestDto<SchoolComment>
                {
                    PagingDto = request.PagingDto,
                    Specification = new SchoolIdEqualsSpecification<SchoolComment>(schoolId)
                        .And(new StatusEqualsSpecification<SchoolComment>(Status.Confirmed)),
                });
                return Ok(new ApiResponse<ListDataSource<SchoolCommentsResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new SchoolCommentsResponseViewModel
                        {
                            Id = t.Id,
                            AverageRate = t.AverageRate,
                            Comment = t.Comment,
                            CreationDate = t.CreationDate,
                            CreationUser = t.CreationUser,
                            CreationUserAvatar = t.CreationUserAvatar,
                            DislikeCount = t.DislikeCount,
                            LikeCount = t.LikeCount,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<SchoolCommentsResponseViewModel>>(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("{schoolId:int}/comments"), Produces<ApiResponse<ManageSchoolCommentResponseViewModel>>()]
        [Permission(policy: null)]
        public async Task<IActionResult<ManageSchoolCommentResponseViewModel>> CreateSchoolComment([FromRoute] int schoolId, [NotNull] ManageSchoolCommentRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.ManageSchoolCommentAsync(new Data.Dto.School.ManageSchoolCommentRequestDto
                {
                    ArtisticActivitiesRate = request.ArtisticActivitiesRate,
                    BehaviorRate = request.BehaviorRate,
                    ClassesQualityRate = request.ClassesQualityRate,
                    Comment = request.Comment,
                    EducationRate = request.EducationRate,
                    FacilitiesRate = request.FacilitiesRate,
                    ITTrainingRate = request.ITTrainingRate,
                    SafetyAndHappinessRate = request.SafetyAndHappinessRate,
                    SchoolId = schoolId,
                    TuitionRatioRate = request.TuitionRatioRate,
                    CreationDate = DateTimeOffset.UtcNow,
                    CreationUserId = User.UserId(),
                });
                return Ok(new ApiResponse<ManageSchoolCommentResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageSchoolCommentResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpPut("{schoolId:int}/comments/{commentId:long}"), Produces<ApiResponse<ManageSchoolCommentResponseViewModel>>()]
        [Permission(policy: null)]
        public async Task<IActionResult<ManageSchoolCommentResponseViewModel>> UpdateSchoolComment([FromRoute] int schoolId, [FromRoute] long commentId, [NotNull, FromBody] ManageSchoolCommentRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.ManageSchoolCommentAsync(new Data.Dto.School.ManageSchoolCommentRequestDto
                {
                    Id = commentId,
                    SchoolId = schoolId,
                    ArtisticActivitiesRate = request.ArtisticActivitiesRate,
                    BehaviorRate = request.BehaviorRate,
                    ClassesQualityRate = request.ClassesQualityRate,
                    Comment = request.Comment,
                    EducationRate = request.EducationRate,
                    FacilitiesRate = request.FacilitiesRate,
                    ITTrainingRate = request.ITTrainingRate,
                    SafetyAndHappinessRate = request.SafetyAndHappinessRate,
                    TuitionRatioRate = request.TuitionRatioRate,
                    CreationDate = DateTimeOffset.UtcNow,
                    CreationUserId = User.UserId(),
                });
                return Ok(new ApiResponse<ManageSchoolCommentResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageSchoolCommentResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpPatch("{schoolId:int}/comments/{commentId:long}/like"), Produces<ApiResponse<bool>>()]
        [Permission(policy: null)]
        public async Task<IActionResult<bool>> LikeSchoolComment([FromRoute] int schoolId, [FromRoute] long commentId)
        {
            try
            {
                var specification = new IdEqualsSpecification<SchoolComment, long>(commentId)
                    .And(new SchoolIdEqualsSpecification<SchoolComment>(schoolId));
                var result = await schoolService.Value.LikeSchoolCommentAsync(specification);
                return Ok(new ApiResponse<bool>
                {
                    Errors = result.Errors,
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool>(new Error { Message = exc.Message }));
            }
        }

        [HttpPatch("{schoolId:int}/comments/{commentId:long}/dislike"), Produces<ApiResponse<bool>>()]
        [Permission(policy: null)]
        public async Task<IActionResult<bool>> DislikeSchoolComment([FromRoute] int schoolId, [FromRoute] long commentId)
        {
            try
            {
                var specification = new IdEqualsSpecification<SchoolComment, long>(commentId)
                    .And(new SchoolIdEqualsSpecification<SchoolComment>(schoolId));
                var result = await schoolService.Value.DislikeSchoolCommentAsync(specification);
                return Ok(new ApiResponse<bool>
                {
                    Errors = result.Errors,
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool>(new Error { Message = exc.Message }));
            }
        }

        #endregion

        #region Images

        [HttpGet("{schoolId:int}/images/{fileType}"), Produces<ApiResponse<IEnumerable<string>>>()]
        public async Task<IActionResult<IEnumerable<string?>>> GetSchoolImagesPath([FromRoute] int schoolId, [FromRoute] FileType fileType)
        {
            try
            {
                var specification = new StatusEqualsSpecification<SchoolImage>(Status.Confirmed)
                    .And(new SchoolIdEqualsSpecification<SchoolImage>(schoolId))
                    .And(new SchoolImageFileTypeEqualsSpecification(fileType));
                var result = await schoolService.Value.GetSchoolImagesPathAsync(specification);
                return Ok(new ApiResponse<IEnumerable<string?>>(result.Errors)
                {
                    Data = result.Data is null ? [] : result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<IEnumerable<string?>>(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("{schoolId:int}/images"), Produces<ApiResponse<CreateSchoolImageResponseViewModel>>()]
        [Permission(policy: null)]
        public async Task<IActionResult<CreateSchoolImageResponseViewModel>> CreateSchoolImage([FromRoute] int schoolId, [NotNull] CreateSchoolImageRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.CreateSchoolImageAsync(new Data.Dto.School.CreateSchoolImageRequestDto
                {
                    File = request.File!,
                    FileType = request.FileType!,
                    SchoolId = schoolId,
                    CreationDate = DateTimeOffset.UtcNow,
                    CreationUserId = User.UserId(),
                });
                return Ok(new ApiResponse<CreateSchoolImageResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<CreateSchoolImageResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        #endregion
    }
}

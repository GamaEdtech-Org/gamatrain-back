namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Collections.Generic;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Data.Dto.Blog;
    using GamaEdtech.Data.Dto.Contribution;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Domain.Specification;
    using GamaEdtech.Domain.Specification.ApplicationSetting;
    using GamaEdtech.Presentation.ViewModel.ApplicationSettings;
    using GamaEdtech.Presentation.ViewModel.Blog;

    using Microsoft.AspNetCore.Mvc;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class BlogsController(Lazy<ILogger<BlogsController>> logger, Lazy<IBlogService> blogService
        , Lazy<IContributionService> contributionService, Lazy<IFileService> fileService, Lazy<IGlobalService> globalService)
        : ApiControllerBase<BlogsController>(logger)
    {
        [HttpGet("contributions"), Produces<ApiResponse<ListDataSource<PostContributionListResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<PostContributionListResponseViewModel>>> GetPostContributionList([NotNull, FromQuery] PostContributionListRequestViewModel request)
        {
            try
            {
                ISpecification<Contribution> specification = new CategoryTypeEqualsSpecification<Contribution>(CategoryType.Post);
                if (request.Status is not null)
                {
                    specification = specification.And(new StatusEqualsSpecification<Contribution>(request.Status));
                }

                var result = await contributionService.Value.GetContributionsAsync<PostContributionDto>(new ListRequestDto<Contribution>
                {
                    PagingDto = request.PagingDto,
                    Specification = specification,
                }, true);
                return Ok(new ApiResponse<ListDataSource<PostContributionListResponseViewModel>>(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new PostContributionListResponseViewModel
                        {
                            Id = t.Id,
                            CreationUser = t.CreationUser,
                            CreationDate = t.CreationDate,
                            Status = t.Status,
                            Title = t.Data?.Title,
                            PostId = t.IdentifierId,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<PostContributionListResponseViewModel>>(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("contributions/{contributionId:long}"), Produces<ApiResponse<PostContributionResponseViewModel>>()]
        public async Task<IActionResult<PostContributionResponseViewModel>> GetPostContribution([FromRoute] long contributionId)
        {
            try
            {
                var specification = new IdEqualsSpecification<Contribution, long>(contributionId)
                    .And(new CategoryTypeEqualsSpecification<Contribution>(CategoryType.Post));
                var contributionResult = await contributionService.Value.GetContributionAsync<PostContributionDto>(specification);
                if (contributionResult.Data?.Data is null)
                {
                    return Ok(new ApiResponse<PostContributionResponseViewModel>(contributionResult.Errors));
                }

                PostContributionResponseViewModel result = new()
                {
                    Title = contributionResult.Data.Data!.Title,
                    Summary = contributionResult.Data.Data!.Summary,
                    Body = contributionResult.Data.Data!.Body,
                    ImageUri = fileService.Value.GetFileUri(contributionResult.Data.Data!.ImageId, ContainerType.Post).Data,
                    PodcastUri = fileService.Value.GetFileUri(contributionResult.Data.Data!.PodcastId, ContainerType.Post).Data,
                    Tags = contributionResult.Data.Data!.Tags,
                    PublishDate = contributionResult.Data.Data!.PublishDate.GetValueOrDefault(),
                    VisibilityType = contributionResult.Data.Data!.VisibilityType!,
                    Keywords = contributionResult.Data.Data!.Keywords,
                    Slug = contributionResult.Data.Data!.Slug,
                };

                return Ok(new ApiResponse<PostContributionResponseViewModel>
                {
                    Data = result,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<PostContributionResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpPatch("contributions/{contributionId:long}/confirm"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> ConfirmPostContribution([FromRoute] long contributionId)
        {
            try
            {
                var result = await blogService.Value.ConfirmPostContributionAsync(new()
                {
                    ContributionId = contributionId,
                });

                return Ok(new ApiResponse<bool>(result.Errors)
                {
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPatch("contributions/{contributionId:long}/reject"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RejectPostContribution([FromRoute] long contributionId, [NotNull, FromBody] RejectContributionRequestViewModel request)
        {
            try
            {
                var result = await contributionService.Value.RejectContributionAsync(new RejectContributionRequestDto
                {
                    Id = contributionId,
                    Comment = request.Comment,
                });
                return Ok(new ApiResponse<bool>(result.Errors)
                {
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("posts/{postId:long}"), Produces<ApiResponse<ManagePostResponseViewModel>>()]
        public async Task<IActionResult<ManagePostResponseViewModel>> UpdatePost([FromRoute] long postId, [NotNull, FromForm] UpdatePostRequestViewModel request)
        {
            try
            {
                ManagePostRequestDto dto = new()
                {
                    Id = postId,
                    Body = request.Body,
                    Image = request.Image,
                    Podcast = request.Podcast,
                    Keywords = request.Keywords,
                    PublishDate = request.PublishDate,
                    Slug = request.Slug,
                    Summary = request.Summary,
                    Tags = request.Tags,
                    Title = request.Title,
                    VisibilityType = request.VisibilityType,
                };
                var result = await blogService.Value.ManagePostAsync(dto);

                return Ok<ManagePostResponseViewModel>(new(result.Errors)
                {
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ManagePostResponseViewModel>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("posts/{postId:long}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RemovePost([FromRoute] long postId)
        {
            try
            {
                var specification = new IdEqualsSpecification<Post, long>(postId);
                var result = await blogService.Value.RemovePostAsync(specification);
                return Ok(new ApiResponse<bool>
                {
                    Errors = result.Errors,
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool> { Errors = [new() { Message = exc.Message }] });
            }
        }

        #region Site Map

        [HttpGet("site-maps"), Produces<ApiResponse<ListDataSource<SiteMapListResponseViewModel>>>()]
        [Display(Name = "Posts Site Maps List")]
        public async Task<IActionResult<ListDataSource<SiteMapListResponseViewModel>>> GetSiteMapsList([NotNull, FromQuery] SiteMapListRequestViewModel request)
        {
            try
            {
                var result = await globalService.Value.GetSiteMapsAsync(new ListRequestDto<SiteMap>
                {
                    PagingDto = request.PagingDto,
                    Specification = new ItemTypeEqualsSpecification(ItemType.Blog),
                });
                if (result.OperationResult is not Constants.OperationResult.Succeeded)
                {
                    return Ok(new ApiResponse<ListDataSource<SiteMapListResponseViewModel>>(result.Errors));
                }

                if (result.Data.List is null)
                {
                    return Ok(new ApiResponse<ListDataSource<SiteMapListResponseViewModel>>());
                }

                var names = await blogService.Value.GetPostsNameAsync(new IdContainsSpecification<Post, long>(result.Data.List.Select(t => t.IdentifierId)));
                if (names.OperationResult is not Constants.OperationResult.Succeeded)
                {
                    return Ok(new ApiResponse<ListDataSource<SiteMapListResponseViewModel>>(names.Errors));
                }

                var lst = result.Data.List.Select(t => new SiteMapListResponseViewModel
                {
                    Id = t.Id,
                    IdentifierId = t.IdentifierId,
                    ChangeFrequency = t.ChangeFrequency,
                    Priority = t.Priority,
                    Title = names.Data?.Find(s => s.Key == t.IdentifierId).Value,
                });
                return Ok(new ApiResponse<ListDataSource<SiteMapListResponseViewModel>>(result.Errors)
                {
                    Data = new()
                    {
                        List = lst,
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<SiteMapListResponseViewModel>>(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("{postId:long}/site-maps"), Produces<ApiResponse<ManageSiteMapResponseViewModel>>()]
        [Display(Name = "Create Post Site Maps")]
        public async Task<IActionResult<ManageSiteMapResponseViewModel>> CreateSiteMap([FromRoute] long postId, [NotNull] ManageSiteMapRequestViewModel request)
        {
            try
            {
                var result = await globalService.Value.ManageSiteMapAsync(new()
                {
                    IdentifierId = postId,
                    ItemType = ItemType.Blog,
                    ChangeFrequency = request.ChangeFrequency,
                    Priority = request.Priority,
                });
                return Ok<ManageSiteMapResponseViewModel>(new(result.Errors)
                {
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ManageSiteMapResponseViewModel>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("{postId:long}/site-maps/{id:long}"), Produces(typeof(ApiResponse<ManageSiteMapResponseViewModel>))]
        [Display(Name = "Edit Post Site Maps")]
        public async Task<IActionResult<ManageSiteMapResponseViewModel>> UpdateSiteMap([FromRoute] long postId, [FromRoute] long id, [NotNull] ManageSiteMapRequestViewModel request)
        {
            try
            {
                var result = await globalService.Value.ManageSiteMapAsync(new()
                {
                    Id = id,
                    IdentifierId = postId,
                    ItemType = ItemType.Blog,
                    ChangeFrequency = request.ChangeFrequency,
                    Priority = request.Priority,
                });
                return Ok<ManageSiteMapResponseViewModel>(new(result.Errors)
                {
                    Data = new() { Id = result.Data }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ManageSiteMapResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpDelete("{postId:long}/site-maps/{id:long}"), Produces<ApiResponse<bool>>()]
        [Display(Name = "Remove Post Site Map")]
        public async Task<IActionResult> RemoveSiteMap([FromRoute] long postId, [FromRoute] long id)
        {
            try
            {
                var specification = new IdEqualsSpecification<SiteMap, long>(id)
                    .And(new ItemTypeEqualsSpecification(ItemType.Blog))
                    .And(new IdentifierIdEqualsSpecification<SiteMap>(postId));
                var result = await globalService.Value.RemoveSiteMapAsync(specification);
                return Ok(new ApiResponse<bool>
                {
                    Errors = result.Errors,
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool> { Errors = [new() { Message = exc.Message }] });
            }
        }

        #endregion
    }
}

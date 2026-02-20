namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using EntityFramework.Exceptions.Common;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Linq;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.ApplicationSettings;
    using GamaEdtech.Data.Dto.Blog;
    using GamaEdtech.Data.Dto.Contribution;
    using GamaEdtech.Data.Dto.SiteMap;
    using GamaEdtech.Data.Dto.Tag;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Domain.Specification;
    using GamaEdtech.Domain.Specification.Post;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class BlogService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<BlogService>> localizer
        , Lazy<ILogger<BlogService>> logger, Lazy<IReactionService> reactionService, Lazy<IFileService> fileService, Lazy<IIdentityService> identityService, Lazy<IEmailService> emailService
        , Lazy<IContributionService> contributionService, Lazy<ITagService> tagService, Lazy<IConfiguration> configuration, Lazy<IApplicationSettingsService> applicationSettingsService)
        : LocalizableServiceBase<BlogService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IBlogService, ISiteMapHandler
    {
        public async Task<ResultData<ListDataSource<PostsDto>>> GetPostsAsync(ListRequestDto<Post>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var lst = await uow.GetRepository<Post>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var blogs = await lst.List.Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Slug,
                    t.Summary,
                    t.LikeCount,
                    t.DislikeCount,
                    t.ImageId,
                    t.PublishDate,
                    t.VisibilityType,
                }).ToListAsync();

                var result = blogs.Select(t => new PostsDto
                {
                    Id = t.Id,
                    DislikeCount = t.DislikeCount,
                    LikeCount = t.LikeCount,
                    Summary = t.Summary,
                    Title = t.Title,
                    Slug = t.Slug,
                    ImageUri = fileService.Value.GetFileUri(t.ImageId, ContainerType.Post).Data,
                    PublishDate = t.PublishDate,
                    VisibilityType = t.VisibilityType,
                });

                return new(OperationResult.Succeeded) { Data = new() { List = result, TotalRecordsCount = lst.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<IReadOnlyList<KeyValuePair<long, string?>>>> GetPostsNameAsync([NotNull] ISpecification<Post> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var name = await uow.GetRepository<Post>().GetManyQueryable(specification).Select(t => new KeyValuePair<long, string?>(t.Id, t.Title)).ToListAsync();

                return new(OperationResult.Succeeded) { Data = name };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<PostDto>> GetPostAsync([NotNull] ISpecification<Post> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Post>();
                var post = await repository.GetManyQueryable(specification).Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Slug,
                    t.Summary,
                    t.Body,
                    t.ImageId,
                    t.PodcastId,
                    t.LikeCount,
                    t.DislikeCount,
                    t.VisibilityType,
                    CreationUser = t.CreationUser.FirstName + " " + t.CreationUser.LastName,
                    t.CreationUser.Avatar,
                    t.PublishDate,
                    t.Keywords,
                    t.ViewCount,
                    Tags = t.PostTags == null ? null : t.PostTags.Select(s => new TagDto
                    {
                        Icon = s.Tag.Icon,
                        Id = s.TagId,
                        Name = s.Tag.Name,
                        TagType = s.Tag.TagType,
                    }),
                }).FirstOrDefaultAsync();
                if (post is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["PostNotFound"] },],
                    };
                }

                var nextId = await repository.GetManyQueryable(new PublishDateSpecification()).Where(t => t.Id > post.Id).OrderBy(t => t.Id).Select(t => (long?)t.Id).FirstOrDefaultAsync();
                var previousId = await repository.GetManyQueryable(new PublishDateSpecification()).Where(t => t.Id > post.Id).OrderByDescending(t => t.Id).Select(t => (long?)t.Id).FirstOrDefaultAsync();

                PostDto result = new()
                {
                    Title = post.Title,
                    Slug = post.Slug,
                    Summary = post.Summary,
                    Body = post.Body,
                    ImageUri = fileService.Value.GetFileUri(post.ImageId, ContainerType.Post).Data,
                    PodcastUri = fileService.Value.GetFileUri(post.PodcastId, ContainerType.Post).Data,
                    LikeCount = post.LikeCount,
                    DislikeCount = post.DislikeCount,
                    CreationUser = post.CreationUser,
                    CreationUserAvatar = post.Avatar,
                    Tags = post.Tags,
                    VisibilityType = post.VisibilityType,
                    PublishDate = post.PublishDate,
                    Keywords = post.Keywords,
                    ViewCount = post.ViewCount,
                    NextId = nextId,
                    PreviousId = previousId,
                };

                return new(OperationResult.Succeeded) { Data = result };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<long>> ManagePostContributionAsync([NotNull] ManagePostContributionRequestDto requestDto)
        {
            try
            {
                long? identifierId = null;
                if (requestDto.ContributionId.HasValue)
                {
                    var specification = new IdEqualsSpecification<Contribution, long>(requestDto.ContributionId.Value)
                        .And(new CreationUserIdEqualsSpecification<Contribution, ApplicationUser, int>(requestDto.UserId))
                        .And(new CategoryTypeEqualsSpecification<Contribution>(CategoryType.Post))
                        .AndNot(new StatusEqualsSpecification<Contribution>(Status.Deleted));
                    var data = await contributionService.Value.ExistsContributionAsync(specification);

                    if (data.OperationResult is not OperationResult.Succeeded)
                    {
                        return new(data.OperationResult) { Errors = data.Errors };
                    }

                    if (!data.Data)
                    {
                        return new(OperationResult.NotValid) { Errors = [new() { Message = "Invalid Blog Status", }] };
                    }
                }

                ISpecification<Post> slugSpecification = new SlugEqualsSpecification(requestDto.Slug!);
                if (requestDto.ContributionId.HasValue)
                {
                    identifierId = (await contributionService.Value.GetIdentifierIdAsync(new IdEqualsSpecification<Contribution, long>(requestDto.ContributionId.Value))).Data;
                    if (identifierId.HasValue)
                    {
                        slugSpecification = slugSpecification.AndNot(new IdEqualsSpecification<Post, long>(identifierId.Value));
                    }
                }

                var exists = await PostExistsAsync(slugSpecification);
                if (exists.Data)
                {
                    return new(OperationResult.Duplicate) { Errors = [new() { Message = Localizer.Value["DuplicateSlug"] },], };
                }

                if (requestDto.Tags?.Any() == true)
                {
                    var count = await tagService.Value.GetTagsCountAsync(new IdContainsSpecification<Tag, long>(requestDto.Tags));
                    if (count.Data != requestDto.Tags.Count())
                    {
                        return new(OperationResult.Duplicate) { Errors = [new() { Message = Localizer.Value["InvalidTag"] },], };
                    }
                }

                var (imageId, imageErrors) = await SaveFileAsync(requestDto.Image);
                if (imageErrors is not null)
                {
                    return new(OperationResult.Failed)
                    {
                        Errors = imageErrors,
                    };
                }

                var (podcastId, podcastErrors) = await SaveFileAsync(requestDto.Podcast);
                if (podcastErrors is not null)
                {
                    return new(OperationResult.Failed)
                    {
                        Errors = podcastErrors,
                    };
                }

                PostContributionDto dto = new()
                {
                    CreationDate = DateTimeOffset.UtcNow,
                    CreationUserId = requestDto.UserId,
                    Body = requestDto.Body,
                    ImageId = imageId,
                    PodcastId = podcastId,
                    RemovePodcast = requestDto.RemovePodcast,
                    Summary = requestDto.Summary,
                    Tags = requestDto.Tags,
                    Title = requestDto.Title,
                    Slug = requestDto.Slug,
                    PublishDate = requestDto.PublishDate,
                    VisibilityType = requestDto.VisibilityType,
                    Keywords = requestDto.Keywords,
                    Draft = requestDto.Draft,
                };

                var contributionResult = await contributionService.Value.ManageContributionAsync(new ManageContributionRequestDto<PostContributionDto>
                {
                    CategoryType = CategoryType.Post,
                    IdentifierId = identifierId,
                    Status = requestDto.Draft.GetValueOrDefault() ? Status.Draft : Status.Review,
                    Data = dto,
                    Id = requestDto.ContributionId,
                });
                if (contributionResult.OperationResult is not OperationResult.Succeeded)
                {
                    return new(contributionResult.OperationResult) { Errors = contributionResult.Errors };
                }

                if (!requestDto.Draft.GetValueOrDefault())
                {
                    var hasAutoConfirmSchoolComment = await identityService.Value.HasClaimAsync(requestDto.UserId, SystemClaim.AutoConfirmPost);
                    if (hasAutoConfirmSchoolComment.Data || configuration.Value.GetValue<bool>("AutoConfirmPosts"))
                    {
                        _ = await ConfirmPostContributionAsync(new()
                        {
                            ContributionId = contributionResult.Data,
                            NotifyUser = false,
                        });
                    }
                }

                return new(OperationResult.Succeeded) { Data = contributionResult.Data };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<long>> ManagePostAsync([NotNull] ManagePostRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Post>();
                Post? post = null;

                if (string.IsNullOrEmpty(requestDto.ImageId))
                {
                    var (imageId, errors) = await SaveFileAsync(requestDto.Image);
                    if (errors is not null)
                    {
                        return new(OperationResult.Failed)
                        {
                            Errors = errors,
                        };
                    }
                    requestDto.ImageId = imageId;
                }

                if (string.IsNullOrEmpty(requestDto.PodcastId))
                {
                    var (podcastId, errors) = await SaveFileAsync(requestDto.Podcast);
                    if (errors is not null)
                    {
                        return new(OperationResult.Failed)
                        {
                            Errors = errors,
                        };
                    }
                    requestDto.PodcastId = podcastId;
                }

                if (requestDto.Id.HasValue)
                {
                    post = await repository.GetAsync(requestDto.Id.Value, includes: (t) => t.Include(s => s.PostTags));
                    if (post is null)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["PostNotFound"] },],
                        };
                    }

                    if (requestDto.RemovePodcast)
                    {
                        _ = await fileService.Value.RemoveFileAsync(new()
                        {
                            FileId = requestDto.PodcastId,
                            ContainerType = ContainerType.Post,
                        });
                    }

                    post.Slug = requestDto.Slug ?? post.Slug;
                    post.Title = requestDto.Title ?? post.Title;
                    post.Summary = requestDto.Summary ?? post.Summary;
                    post.Body = requestDto.Body ?? post.Body;
                    post.ImageId = requestDto.ImageId ?? post.ImageId;
                    post.PodcastId = requestDto.RemovePodcast ? null : (requestDto.PodcastId ?? post.PodcastId);
                    post.PublishDate = requestDto.PublishDate ?? post.PublishDate;
                    post.VisibilityType = requestDto.VisibilityType ?? post.VisibilityType;
                    post.Keywords = requestDto.Keywords ?? post.Keywords;

                    _ = repository.Update(post);

                    if (requestDto.Tags?.Any() == true)
                    {
                        var postTagRepository = uow.GetRepository<PostTag>();

                        var removedTags = post.PostTags?.Where(t => requestDto.Tags is null || !requestDto.Tags.Contains(t.TagId));
                        var newTags = requestDto.Tags?.Where(t => post.PostTags is null || post.PostTags.All(s => s.TagId != t));

                        if (removedTags is not null)
                        {
                            foreach (var item in removedTags)
                            {
                                postTagRepository.Remove(item);
                            }
                        }

                        if (newTags is not null)
                        {
                            foreach (var item in newTags)
                            {
                                postTagRepository.Add(new PostTag
                                {
                                    PostId = requestDto.Id.Value,
                                    TagId = item,
                                    CreationDate = DateTimeOffset.UtcNow,
                                    CreationUserId = HttpContextAccessor.Value.HttpContext.UserId(),
                                });
                            }
                        }
                    }
                }
                else
                {
                    post = new Post
                    {
                        Slug = requestDto.Slug,
                        Title = requestDto.Title,
                        Summary = requestDto.Summary,
                        Body = requestDto.Body,
                        PublishDate = requestDto.PublishDate.GetValueOrDefault(),
                        VisibilityType = requestDto.VisibilityType!,
                        Keywords = requestDto.Keywords,
                        ImageId = requestDto.ImageId,
                        PodcastId = requestDto.PodcastId,
                        CreationUserId = requestDto.CreationUserId,
                        CreationDate = requestDto.CreationDate,
                    };
                    if (requestDto.Tags is not null)
                    {
                        post.PostTags = [.. requestDto.Tags.Select(t => new PostTag {
                            TagId = t,
                            CreationUserId = HttpContextAccessor.Value.HttpContext.UserId(),
                            CreationDate = DateTimeOffset.UtcNow,
                        })];
                    }
                    repository.Add(post);
                }

                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = post.Id };
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

        public async Task<ResultData<bool>> LikePostAsync([NotNull] PostReactionRequestDto requestDto)
        {
            try
            {
                var specification = new IdEqualsSpecification<Post, long>(requestDto.PostId);
                var exists = await PostExistsAsync(specification);
                if (!exists.Data)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["InvalidRequest"] },],
                    };
                }

                var reactionResult = await reactionService.Value.ManageReactionAsync(new()
                {
                    CategoryType = CategoryType.Post,
                    CreationDate = DateTimeOffset.UtcNow,
                    CreationUserId = HttpContextAccessor.Value.HttpContext.UserId(),
                    IdentifierId = requestDto.PostId,
                    IsLike = true,
                });
                if (reactionResult.OperationResult is not OperationResult.Succeeded)
                {
                    return new(OperationResult.Failed) { Errors = reactionResult.Errors };
                }

                var result = await UpdatePostReactionsAsync(requestDto.PostId);

                return result;
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> DislikePostAsync([NotNull] PostReactionRequestDto requestDto)
        {
            try
            {
                var specification = new IdEqualsSpecification<Post, long>(requestDto.PostId);
                var exists = await PostExistsAsync(specification);
                if (!exists.Data)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["InvalidRequest"] },],
                    };
                }

                var reactionResult = await reactionService.Value.ManageReactionAsync(new()
                {
                    CategoryType = CategoryType.Post,
                    CreationDate = DateTimeOffset.UtcNow,
                    CreationUserId = HttpContextAccessor.Value.HttpContext.UserId(),
                    IdentifierId = requestDto.PostId,
                    IsLike = false,
                });
                if (reactionResult.OperationResult is not OperationResult.Succeeded)
                {
                    return new(OperationResult.Failed) { Errors = reactionResult.Errors };
                }

                var result = await UpdatePostReactionsAsync(requestDto.PostId);

                return result;
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> RemovePostAsync([NotNull] ISpecification<Post> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var postRepository = uow.GetRepository<Post>();
                var post = await postRepository.GetAsync(specification);
                if (post is null)
                {
                    return new(OperationResult.NotFound) { Errors = [new() { Message = Localizer.Value["PostNotFound"] },], };
                }

                //remove post
                postRepository.Remove(post);
                _ = await uow.SaveChangesAsync();

                //remove reactions
                var reactionSpecification = new IdentifierIdEqualsSpecification<Reaction>(post.Id)
                    .And(new CategoryTypeEqualsSpecification<Reaction>(CategoryType.Post));
                _ = reactionService.Value.RemoveReactionAsync(reactionSpecification);

                //remove image
                _ = await fileService.Value.RemoveFileAsync(new()
                {
                    ContainerType = ContainerType.Post,
                    FileId = post.ImageId,
                });

                //remove Podcast
                _ = await fileService.Value.RemoveFileAsync(new()
                {
                    ContainerType = ContainerType.Post,
                    FileId = post.PodcastId,
                });

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["PostCantBeRemoved"], },] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> PostExistsAsync([NotNull] ISpecification<Post> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var exists = await uow.GetRepository<Post>().AnyAsync(specification);

                return new(OperationResult.Succeeded) { Data = exists };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> ConfirmPostContributionAsync([NotNull] ConfirmPostContributionRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                using var scope = uow.CreateTransactionScope();

                var contributionSpecification = new IdEqualsSpecification<Contribution, long>(requestDto.ContributionId)
                    .And(new CategoryTypeEqualsSpecification<Contribution>(CategoryType.Post));
                var result = await contributionService.Value.ConfirmContributionAsync<PostContributionDto>(new()
                {
                    Specification = contributionSpecification,
                });
                if (result.Data is null)
                {
                    return new(OperationResult.Failed) { Errors = result.Errors };
                }

                if (result.Data.IdentifierId.HasValue)
                {
                    _ = await ManagePostAsync(new()
                    {
                        Body = result.Data.Data!.Body,
                        CreationUserId = result.Data.Data!.CreationUserId.GetValueOrDefault(),
                        CreationDate = result.Data.Data!.CreationDate.GetValueOrDefault(),
                        ImageId = result.Data.Data!.ImageId,
                        PodcastId = result.Data.Data!.PodcastId,
                        RemovePodcast = result.Data.Data!.RemovePodcast,
                        Title = result.Data.Data!.Title,
                        Slug = result.Data.Data!.Slug,
                        Summary = result.Data.Data!.Summary,
                        PublishDate = result.Data.Data!.PublishDate.GetValueOrDefault(),
                        VisibilityType = result.Data.Data!.VisibilityType!,
                        Keywords = result.Data.Data!.Keywords,
                        Tags = result.Data.Data!.Tags,
                        Id = result.Data.IdentifierId.Value,
                    });
                }
                else
                {
                    Post post = new()
                    {
                        Body = result.Data.Data!.Body,
                        CreationUserId = result.Data.Data!.CreationUserId.GetValueOrDefault(),
                        CreationDate = result.Data.Data!.CreationDate.GetValueOrDefault(),
                        ImageId = result.Data.Data!.ImageId,
                        PodcastId = result.Data.Data!.PodcastId,
                        Title = result.Data.Data!.Title,
                        Slug = result.Data.Data!.Slug,
                        Summary = result.Data.Data!.Summary,
                        PublishDate = result.Data.Data!.PublishDate.GetValueOrDefault(),
                        VisibilityType = result.Data.Data!.VisibilityType!,
                        Keywords = result.Data.Data!.Keywords,
                        PostTags = result.Data.Data!.Tags?.Select(t => new PostTag
                        {
                            CreationUserId = result.Data.Data!.CreationUserId.GetValueOrDefault(),
                            CreationDate = result.Data.Data!.CreationDate.GetValueOrDefault(),
                            TagId = t,
                        }).ToList(),
                    };
                    var postRepository = uow.GetRepository<Post>();
                    postRepository.Add(post);
                    _ = await uow.SaveChangesAsync();

                    _ = await contributionService.Value.UpdateIdentifierIdAsync(requestDto.ContributionId, post.Id);
                }

                if (requestDto.NotifyUser)
                {
                    var template = (await applicationSettingsService.Value.GetSettingAsync<string?>(nameof(ApplicationSettingsDto.PostContributionConfirmationEmailTemplate))).Data;
                    template = template?
                        .Replace("[RECEIVER_NAME]", result.Data.FullName, StringComparison.OrdinalIgnoreCase)
                        .Replace("[POST_TITLE]", result.Data.Data.Title, StringComparison.OrdinalIgnoreCase);
                    _ = await emailService.Value.SendEmailAsync(new()
                    {
                        Subject = "Post Contribution Confirmation",
                        Body = template!,
                        EmailAddresses = [result.Data.Email],
                    });
                }

                scope.Complete();

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> IsCreatorOfPostAsync(long postId, int userId)
        {
            try
            {
                var specification = new IdEqualsSpecification<Post, long>(postId)
                    .And(new CreationUserIdEqualsSpecification<Post, ApplicationUser, int>(userId));

                var exists = await PostExistsAsync(specification);

                return new(OperationResult.Succeeded) { Data = exists.Data };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task IncreasePostViewAsync(long id)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                _ = await uow.GetRepository<Post>().GetManyQueryable(t => t.Id == id).ExecuteUpdateAsync(t => t.SetProperty(p => p.ViewCount, p => p.ViewCount + 1));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
            }
        }

        private async Task<(string? ImageId, IEnumerable<Error>? Errors)> SaveFileAsync(IFormFile? file)
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
                ContainerType = ContainerType.Post,
                FileExtension = Path.GetExtension(file.FileName),
            });

            return fileId.OperationResult is OperationResult.Succeeded
                ? ((string? ImageId, IEnumerable<Error>? Errors))(fileId.Data, null)
                : new(null, fileId.Errors);
        }

        #region SiteMap

        public ItemType ItemType => ItemType.Blog;

        public IQueryable<SiteMapItemDto> GetSiteMapData([NotNull] IUnitOfWork uow)
        {
            var now = DateTimeOffset.UtcNow;
            return uow.GetRepository<Post>().GetManyQueryable(t => t.PublishDate <= now && t.VisibilityType == VisibilityType.General).Select(t => new SiteMapItemDto
            {
                Id = t.Id,
                Title = t.Slug ?? t.Title,
                LastModifyDate = t.LastModifyDate ?? t.CreationDate,
            });
        }

        #endregion

        #region Job

        public async Task<ResultData<bool>> UpdatePostReactionsAsync(long? postId = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var where = postId.HasValue ? $"WHERE p.Id={postId.Value}" : "";
                var query = $@"UPDATE p SET
                    LikeCount=(SELECT COUNT(1) FROM Reactions r WHERE r.CategoryType={CategoryType.Post.Value} AND r.IdentifierId=p.Id AND r.IsLike=1)
                    ,DislikeCount=(SELECT COUNT(1) FROM Reactions r WHERE r.CategoryType={CategoryType.Post.Value} AND r.IdentifierId=p.Id AND r.IsLike=0)
                FROM Posts p {where}";
                _ = await uow.ExecuteSqlCommandAsync(query);

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        #endregion
    }
}

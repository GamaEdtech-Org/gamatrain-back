namespace GamaEdtech.Presentation.Api.Controllers
{
    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Domain.Specification.Tag;
    using GamaEdtech.Presentation.ViewModel.Tag;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TagsController(Lazy<ILogger<TagsController>> logger, Lazy<ITagService> tagService)
        : ApiControllerBase<TagsController>(logger)
    {
        [HttpGet("{tagType:TagType}"), Produces<ApiResponse<IEnumerable<TagsResponseViewModel>>>()]
        public async Task<IActionResult<IEnumerable<TagsResponseViewModel>>> GetTags([FromRoute] TagType tagType)
        {
            try
            {
                var result = await tagService.Value.GetTagsAsync(new ListRequestDto<Tag>
                {
                    PagingDto = new() { PageFilter = new() { Size = 100 } },
                    Specification = new TagTypeEqualsSpecification(tagType),
                });
                return Ok<IEnumerable<TagsResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data.List?.Select(t => new TagsResponseViewModel
                    {
                        Id = t.Id,
                        Icon = t.Icon,
                        Name = t.Name,
                        TagType = t.TagType,
                    }),
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<IEnumerable<TagsResponseViewModel>>(new(new Error { Message = exc.Message }));
            }
        }
    }
}

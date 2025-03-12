namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.DataAnnotation;

    using GamaEdtech.Data.Dto.Tag;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface ITagService
    {
        Task<ResultData<ListDataSource<TagsDto>>> GetTagsAsync(ListRequestDto<Tag>? requestDto = null);
        Task<ResultData<TagDto>> GetTagAsync([NotNull] ISpecification<Tag> specification);
        Task<ResultData<int>> ManageTagAsync([NotNull] ManageTagRequestDto requestDto);
        Task<ResultData<bool>> RemoveTagAsync([NotNull] ISpecification<Tag> specification);
    }
}

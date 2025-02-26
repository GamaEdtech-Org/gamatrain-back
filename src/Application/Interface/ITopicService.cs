namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.DataAnnotation;

    using GamaEdtech.Data.Dto.Topic;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface ITopicService
    {
        Task<ResultData<ListDataSource<TopicsDto>>> GetTopicsAsync(ListRequestDto<Topic>? requestDto = null);
        Task<ResultData<TopicDto>> GetTopicAsync([NotNull] ISpecification<Topic> specification);
        Task<ResultData<int>> ManageTopicAsync([NotNull] ManageTopicRequestDto requestDto);
        Task<ResultData<bool>> RemoveTopicAsync([NotNull] ISpecification<Topic> specification);
    }
}

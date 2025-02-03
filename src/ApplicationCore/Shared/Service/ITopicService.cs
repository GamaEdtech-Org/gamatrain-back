namespace GamaEdtech.Backend.Shared.Service
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.DataAnnotation;

    using GamaEdtech.Backend.Data.Dto.Topic;
    using GamaEdtech.Backend.Data.Entity;

    [Injectable]
    public interface ITopicService
    {
        Task<ResultData<ListDataSource<TopicsDto>>> GetTopicsAsync(ListRequestDto<Topic>? requestDto = null);
        Task<ResultData<TopicDto>> GetTopicAsync([NotNull] ISpecification<Topic> specification);
        Task<ResultData<int>> ManageTopicAsync([NotNull] ManageTopicRequestDto requestDto);
        Task<ResultData<bool>> RemoveTopicAsync([NotNull] ISpecification<Topic> specification);
    }
}

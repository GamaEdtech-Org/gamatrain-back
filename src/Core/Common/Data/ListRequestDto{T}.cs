namespace GamaEdtech.Common.Data
{
    using GamaEdtech.Common.DataAccess.Specification;

    public class ListRequestDto<T>
    {
        public ISpecification<T>? Specification { get; set; }

        public PagingDto? PagingDto { get; set; }
    }
}

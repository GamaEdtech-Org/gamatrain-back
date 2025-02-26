namespace GamaEdtech.Common.Data
{
    using System.Collections.Generic;

    public class PagingDto
    {
        public PageFilter? PageFilter { get; set; }

        public IEnumerable<SortFilter>? SortFilter { get; set; }

        public IEnumerable<SearchFilter>? SearchFilter { get; set; }
    }
}

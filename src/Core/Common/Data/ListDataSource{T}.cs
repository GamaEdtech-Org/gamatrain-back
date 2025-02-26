namespace GamaEdtech.Common.Data
{
    using System.Collections.Generic;

    public struct ListDataSource<T>
    {
        public IEnumerable<T>? List { get; set; }

        public int? TotalRecordsCount { get; set; }
    }
}

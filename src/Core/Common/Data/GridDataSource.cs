namespace GamaEdtech.Common.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Core;

    public class GridDataSource
    {
        [JsonPropertyName("current")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("total")]
        public int? TotalRecords { get; set; } = Constants.TotalRecords;

        [JsonPropertyName("rows")]
        public IList? Data { get; set; }

        [JsonPropertyName("columns")]
        public Dictionary<string, string>? Captions { get; set; }

        [JsonIgnore]
        public DataTable? DataTable { get; set; }

        [JsonPropertyName("rowCount")]
        public int PageItemCount { get; set; }

        [JsonPropertyName("sort")]
        public SortFilter? SortFilter { get; set; }
    }
}

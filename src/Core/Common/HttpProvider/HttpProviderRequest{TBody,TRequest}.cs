namespace GamaEdtech.Common.HttpProvider
{
    using System.Collections.Generic;

    public sealed class HttpProviderRequest<TBody, TRequest>
    {
#pragma warning disable SA1206 // Declaration keywords should follow order
        public required TRequest? Request { get; init; }

        public required string? Uri { get; set; }

#pragma warning restore SA1206 // Declaration keywords should follow order

        public string? BaseAddress { get; set; }

        public IReadOnlyList<(string Key, string Value)>? HeaderParameters { get; set; }

        public TBody? Body { get; set; }

        public bool ForceTls13 { get; set; }

        /// <summary>
        /// Gets or sets Timeout in seconds.
        /// </summary>
        public int? Timeout { get; set; }
    }
}

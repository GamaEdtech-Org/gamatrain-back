namespace GamaEdtech.Common.Data
{
    using System;

    public sealed record TimeZoneDto
    {
        public string? Id { get; set; }

        public TimeSpan BaseUtcOffset { get; set; }

        public string? DisplayName { get; set; }
    }
}

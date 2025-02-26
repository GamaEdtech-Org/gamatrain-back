namespace GamaEdtech.Common.Data
{
    using System;

    public struct Language
    {
        public short Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Icon { get; set; }

        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}

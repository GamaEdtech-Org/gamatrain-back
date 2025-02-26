namespace GamaEdtech.Common.TimeZone
{
    using System.Collections.Generic;

    using GamaEdtech.Common.DataAnnotation;

    using GamaEdtech.Common.Data;

    [Injectable]
    public interface ITimeZoneProvider
    {
        IEnumerable<TimeZoneDto>? GetTimeZones();
    }
}

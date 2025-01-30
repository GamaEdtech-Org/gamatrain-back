namespace GamaEdtech.Backend.Data.Dto.ApplicationSettings
{
    public sealed class ApplicationSettingsDto
    {
        #region General

        public int GridPageSize { get; set; } = 10;

        public bool SwaggerEnabled { get; set; }

        public string? DefaultTimeZoneId { get; set; }

        public string? ApplyLimitsCronJob { get; set; }

        #endregion
    }
}

namespace GamaEdtech.Backend.Data.Dto.Identity
{
    using GamaEdtech.Backend.Data.Enumeration;

    public sealed class UserPermissionsResponseDto
    {
        public IEnumerable<string?>? Claims { get; set; }

        public Role? Roles { get; set; }
    }
}

namespace GamaEdtech.Data.Dto.Identity
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class UserPermissionsResponseDto
    {
        public IEnumerable<string?>? Permissions { get; set; }
        public SystemClaim? SystemClaims { get; set; }
        public Role? Roles { get; set; }
    }
}

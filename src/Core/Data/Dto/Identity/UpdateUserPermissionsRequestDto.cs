namespace GamaEdtech.Data.Dto.Identity
{
    using System.Collections.Generic;

    using GamaEdtech.Domain.Enumeration;

    public sealed class UpdateUserPermissionsRequestDto
    {
        public int UserId { get; set; }

        public required IEnumerable<string?> Permissions { get; set; }

        public SystemClaim? SystemClaims { get; set; }

        public Role? Roles { get; set; }
    }
}

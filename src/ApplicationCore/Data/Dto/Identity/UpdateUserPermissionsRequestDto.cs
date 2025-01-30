namespace GamaEdtech.Backend.Data.Dto.Identity
{
    using GamaEdtech.Backend.Data.Enumeration;

    using System.Collections.Generic;

    public sealed class UpdateUserPermissionsRequestDto
    {
        public int UserId { get; set; }

        public IEnumerable<string>? Claims { get; set; }

        public Role? Roles { get; set; }
    }
}

namespace GamaEdtech.Data.Dto.Identity
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class UserInformationResponseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public GenderType? Gender { get; set; }
        public int? Grade { get; set; }
    }
}

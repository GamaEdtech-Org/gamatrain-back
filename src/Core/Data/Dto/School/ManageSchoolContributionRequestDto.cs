namespace GamaEdtech.Data.Dto.School
{
    using Microsoft.AspNetCore.Http;

    public sealed class ManageSchoolContributionRequestDto
    {
        public long? Id { get; set; }
        public long? SchoolId { get; set; }
        public int UserId { get; set; }
        public IFormFile? File { get; set; }
        public bool IsDefault { get; set; }

        public SchoolContributionDto SchoolContribution { get; set; }
    }
}

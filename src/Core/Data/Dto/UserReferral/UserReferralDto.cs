namespace GamaEdtech.Data.Dto.UserReferral
{


    public class UserReferralDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string ReferralId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}

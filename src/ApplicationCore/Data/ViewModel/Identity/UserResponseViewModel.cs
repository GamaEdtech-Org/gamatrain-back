namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    public sealed class UserResponseViewModel
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTimeOffset? RegistrationDate { get; set; }

        public bool Enabled { get; set; }

        public bool LocalAccount { get; set; }
    }
}

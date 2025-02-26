namespace GamaEdtech.Presentation.ViewModel.Identity
{
    public sealed class UserListResponseViewModel
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public bool Enabled { get; set; }

        public bool LocalAccount { get; set; }
    }
}

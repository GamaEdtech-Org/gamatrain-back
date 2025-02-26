namespace GamaEdtech.Presentation.ViewModel.Identity
{
    public sealed class GenerateTokenResponseViewModel
    {
        public string? Token { get; set; }

        public DateTimeOffset? ExpirationTime { get; set; }
    }
}

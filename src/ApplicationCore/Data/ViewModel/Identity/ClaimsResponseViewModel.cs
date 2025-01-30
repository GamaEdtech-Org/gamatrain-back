namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    public sealed class ClaimsResponseViewModel
    {
        public string? Value { get; set; }

        public string? Text { get; set; }

        public IEnumerable<ClaimsResponseViewModel>? Items { get; set; }

        public bool HasPermission { get; set; }
    }
}

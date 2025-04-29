namespace GamaEdtech.Presentation.ViewModel.Identity
{
    public sealed class PermissionsResponseViewModel
    {
        public string? Value { get; set; }

        public string? Text { get; set; }

        public IEnumerable<PermissionsResponseViewModel>? Items { get; set; }

        public bool HasPermission { get; set; }
    }
}

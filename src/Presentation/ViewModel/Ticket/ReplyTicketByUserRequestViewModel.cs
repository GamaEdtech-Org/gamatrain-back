namespace GamaEdtech.Presentation.ViewModel.Ticket
{
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Http;

    public sealed class ReplyTicketByUserRequestViewModel
    {
        [Display]
        [Required]
        public string? Body { get; set; }

        [Display]
        [FileSize(1024 * 1024 * 2)]//2MB
        [FileExtensions(Constants.ValidWebExtensions + "," + Constants.ValidImageExtensions)]
        public IFormFile? File { get; set; }
    }
}

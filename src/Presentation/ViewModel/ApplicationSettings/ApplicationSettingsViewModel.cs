namespace GamaEdtech.Presentation.ViewModel.ApplicationSettings
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ApplicationSettingsViewModel
    {
        [Display]
        [Required]
        public int? GridPageSize { get; set; }

        [Display]
        [Required]
        public string? DefaultTimeZoneId { get; set; }

        [Display]
        [Required]
        public long? SchoolContributionPoints { get; set; }

        [Display]
        [Required]
        public long? SchoolImageContributionPoints { get; set; }

        [Display]
        [Required]
        public long? SchoolCommentContributionPoints { get; set; }

        [Display]
        [Required]
        public long? PostContributionPoints { get; set; }

        [Display]
        [Required]
        public long? SchoolIssuesContributionPoints { get; set; }

        [Display]
        [Required]
        public long? RemoveSchoolImageContributionPoints { get; set; }

        [Display]
        [Required]
        public long? EasterEggBronzePoints { get; set; }

        [Display]
        [Required]
        public long? EasterEggSilverPoints { get; set; }

        [Display]
        [Required]
        public long? EasterEggGoldPoints { get; set; }

        [Display]
        [Required]
        public long? TestTimeCorrectSubmissionPoints { get; set; }

        [Display]
        [Required]
        public long? TestTimeIncorrectSubmissionPoints { get; set; }

        [Display]
        [Required]
        public long? ExamCorrectTestSubmissionPoints { get; set; }

        [Display]
        [Required]
        public long? ExamIncorrectTestSubmissionPoints { get; set; }

        [Display]
        [Required]
        [RequiredTokens("[RECEIVER_NAME]", "[SCHOOL_NAME]", "[COMMENT]")]
        public string? SchoolCommentContributionConfirmationEmailTemplate { get; set; }

        [Display]
        [Required]
        [RequiredTokens("[RECEIVER_NAME]", "[SCHOOL_NAME]")]
        public string? SchoolImageContributionConfirmationEmailTemplate { get; set; }

        [Display]
        [Required]
        [RequiredTokens("[RECEIVER_NAME]", "[SCHOOL_NAME]")]
        public string? RemoveSchoolImageContributionConfirmationEmailTemplate { get; set; }

        [Display]
        [Required]
        [RequiredTokens("[RECEIVER_NAME]", "[SCHOOL_NAME]")]
        public string? SchoolContributionConfirmationEmailTemplate { get; set; }

        [Display]
        [Required]
        [RequiredTokens("[RECEIVER_NAME]", "[SCHOOL_NAME]", "[ISSUES]")]
        public string? SchoolIssuesContributionConfirmationEmailTemplate { get; set; }

        [Display]
        [Required]
        [RequiredTokens("[RECEIVER_NAME]", "[POST_TITLE]")]
        public string? PostContributionConfirmationEmailTemplate { get; set; }

        [Display]
        [Required]
        [RequiredTokens("[RECEIVER_NAME]", "[BODY]")]
        public string? TicketConfirmationEmailTemplate { get; set; }
    }
}

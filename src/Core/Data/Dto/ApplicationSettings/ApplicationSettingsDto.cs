namespace GamaEdtech.Data.Dto.ApplicationSettings
{
    public sealed class ApplicationSettingsDto
    {
        public int GridPageSize { get; set; } = 10;
        public string? DefaultTimeZoneId { get; set; }
        public long SchoolContributionPoints { get; set; }
        public long SchoolImageContributionPoints { get; set; }
        public long SchoolCommentContributionPoints { get; set; }
        public long PostContributionPoints { get; set; }
        public long SchoolIssuesContributionPoints { get; set; }
        public long RemoveSchoolImageContributionPoints { get; set; }
        public long EasterEggBronzePoints { get; set; } = 1000000;
        public long EasterEggSilverPoints { get; set; } = 3000000;
        public long EasterEggGoldPoints { get; set; } = 6000000;
        public long TestTimeCorrectSubmissionPoints { get; set; } = 10;
        public long TestTimeIncorrectSubmissionPoints { get; set; } = 10;
        public long ExamCorrectTestSubmissionPoints { get; set; } = 1000;
        public long ExamIncorrectTestSubmissionPoints { get; set; } = 1000;
        public string? TicketConfirmationEmailTemplate { get; set; } = "Hi [RECEIVER_NAME],<br><br>We received your request<hr><br><br>[BODY]";
        public string? SchoolCommentContributionConfirmationEmailTemplate { get; set; } = "Hi [RECEIVER_NAME],<br><br>your Comment Contribution for [SCHOOL_NAME] has been confirmed<br>[COMMENT]";
        public string? SchoolImageContributionConfirmationEmailTemplate { get; set; } = "Hi [RECEIVER_NAME],<br><br>your Image Contribution for [SCHOOL_NAME] has been confirmed";
        public string? RemoveSchoolImageContributionConfirmationEmailTemplate { get; set; } = "Hi [RECEIVER_NAME],<br><br>your Remove Image Contribution for [SCHOOL_NAME] has been confirmed";
        public string? SchoolContributionConfirmationEmailTemplate { get; set; } = "Hi [RECEIVER_NAME],<br><br>your Contribution for [SCHOOL_NAME] has been confirmed";
        public string? SchoolIssuesContributionConfirmationEmailTemplate { get; set; } = "Hi [RECEIVER_NAME],<br><br>your Contribution for [SCHOOL_NAME] has been confirmed<br>[ISSUES]";
        public string? PostContributionConfirmationEmailTemplate { get; set; } = "Hi [RECEIVER_NAME],<br><br>your Post Contribution has been confirmed<br>[POST_TITLE]";
    }
}

namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class CategoryType : Enumeration<CategoryType, byte>
    {
        [Display]
        public static readonly CategoryType School = new(nameof(School), 1, "SchoolContributionPoints");

        [Display]
        public static readonly CategoryType SchoolComment = new(nameof(SchoolComment), 2, "SchoolCommentContributionPoints");

        [Display]
        public static readonly CategoryType SchoolImage = new(nameof(SchoolImage), 3, "SchoolImageContributionPoints");

        [Display]
        public static readonly CategoryType Post = new(nameof(Post), 4, "PostContributionPoints");

        [Display]
        public static readonly CategoryType SchoolIssues = new(nameof(SchoolIssues), 5, "SchoolIssuesContributionPoints");

        [Display]
        public static readonly CategoryType RemoveSchoolImage = new(nameof(RemoveSchoolImage), 6, "RemoveSchoolImageContributionPoints");

        public string ApplicationSettingsName { get; set; }

        public CategoryType()
        {
        }

        public CategoryType(string name, byte value, string applicationSettingsName)
            : base(name, value) => ApplicationSettingsName = applicationSettingsName;
    }
}

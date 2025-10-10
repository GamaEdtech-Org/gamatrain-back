namespace GamaEdtech.Domain.Entity.Identity
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(ApplicationUser))]
    [Audit((int)Common.Core.Constants.EntityType.ApplicationUser)]
    public class ApplicationUser : IdentityUser<int>, IEntity<ApplicationUser, int>, IEnablable
    {
        public ApplicationUser()
        {
            UserRoles = [];
            UserLogins = [];
            UserClaims = [];
            UserTokens = [];

            SecurityStamp = Guid.NewGuid().ToString();
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }

        public ApplicationUser(string userName)
            : this() => UserName = userName;

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column(nameof(Id), DataType.Int)]
        public override int Id { get; set; }

        [StringLength(256)]
        [Required]
        [Column(nameof(UserName), DataType.UnicodeString)]
        public override string? UserName { get; set; }

        [StringLength(256)]
        [Required]
        [Column(nameof(NormalizedUserName), DataType.UnicodeString)]
        public override string? NormalizedUserName { get; set; }

        [StringLength(256)]
        [Column(nameof(Email), DataType.UnicodeString)]
        public override string? Email { get; set; }

        [StringLength(256)]
        [Column(nameof(NormalizedEmail), DataType.UnicodeString)]
        public override string? NormalizedEmail { get; set; }

        [Required]
        [Column(nameof(EmailConfirmed), DataType.Boolean)]
        public override bool EmailConfirmed { get; set; }

        [StringLength(512)]
        [Column(nameof(PasswordHash), DataType.UnicodeString)]
        public override string? PasswordHash { get; set; }

        [StringLength(50)]
        [Required]
        [Column(nameof(SecurityStamp), DataType.String)]
        [AuditIgnore]
        public override string? SecurityStamp { get; set; }

        [StringLength(50)]
        [Required]
        [Column(nameof(ConcurrencyStamp), DataType.String)]
        [AuditIgnore]
        public override string? ConcurrencyStamp { get; set; }

        [StringLength(50)]
        [Column(nameof(PhoneNumber), DataType.String)]
        public override string? PhoneNumber { get; set; }

        [Required]
        [Column(nameof(PhoneNumberConfirmed), DataType.Boolean)]
        public override bool PhoneNumberConfirmed { get; set; }

        [Required]
        [Column(nameof(TwoFactorEnabled), DataType.Boolean)]
        public override bool TwoFactorEnabled { get; set; }

        [Column(nameof(LockoutEnd), DataType.DateTimeOffset)]
        [AuditIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [Required]
        [Column(nameof(LockoutEnabled), DataType.Boolean)]
        public override bool LockoutEnabled { get; set; }

        [Required]
        [Column(nameof(AccessFailedCount), DataType.Int)]
        [AuditIgnore]
        public override int AccessFailedCount { get; set; }

        [Column(nameof(RegistrationDate), DataType.DateTimeOffset)]
        public DateTimeOffset? RegistrationDate { get; set; }

        [Required]
        [Column(nameof(Enabled), DataType.Boolean)]
        public bool Enabled { get; set; }

        [Column(nameof(FirstName), DataType.UnicodeString)]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Column(nameof(LastName), DataType.UnicodeString)]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Column(nameof(Avatar), DataType.UnicodeMaxString)]
        public string? Avatar { get; set; }

        [Column(nameof(CityId), DataType.Int)]
        public int? CityId { get; set; }
        public Location? City { get; set; }

        [Column(nameof(SchoolId), DataType.Long)]
        public long? SchoolId { get; set; }
        public School? School { get; set; }

        [Column(nameof(ReferralId), DataType.String)]
        [StringLength(10)]
        public string? ReferralId { get; set; }

        [Column(nameof(Gender), DataType.Byte)]
        public GenderType? Gender { get; set; }

        [Column(nameof(Board), DataType.Int)]
        public int? Board { get; set; }

        [Column(nameof(Grade), DataType.Int)]
        public int? Grade { get; set; }

        [Column(nameof(Group), DataType.Int)]
        public int? Group { get; set; }

        [Column(nameof(CoreId), DataType.Int)]
        public int? CoreId { get; set; }

        [Column(nameof(CurrentBalance), DataType.Long)]
        [Required]
        public long CurrentBalance { get; set; }

        [Column(nameof(ProfileUpdated), DataType.Boolean)]
        [Required]
        public bool ProfileUpdated { get; set; }

        public ICollection<ApplicationUserClaim>? UserClaims { get; set; }

        public ICollection<ApplicationUserLogin>? UserLogins { get; set; }

        public ICollection<ApplicationUserRole>? UserRoles { get; set; }

        public ICollection<ApplicationUserToken>? UserTokens { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<ApplicationUser> builder)
        {
            _ = builder.OwnEnumeration<ApplicationUser, GenderType, byte>(t => t.Gender);

            _ = builder.HasIndex(e => e.NormalizedEmail)
                .HasDatabaseName(DbProviderFactories.GetFactory.GetObjectName($"IX_{nameof(ApplicationUser)}_{nameof(NormalizedEmail)}"));

            _ = builder.HasIndex(e => e.NormalizedUserName)
                .HasDatabaseName(DbProviderFactories.GetFactory.GetObjectName($"IX_{nameof(ApplicationUser)}_{nameof(NormalizedUserName)}"))
                .IsUnique()
                .HasFilter($"([{DbProviderFactories.GetFactory.GetObjectName(nameof(NormalizedUserName), pluralize: false)}] IS NOT NULL)");

            _ = builder.HasIndex(e => e.ReferralId)
                .HasDatabaseName(DbProviderFactories.GetFactory.GetObjectName(
                    $"IX_{nameof(ApplicationUser)}_{nameof(ReferralId)}"))
                .IsUnique()
                .HasFilter($"([{DbProviderFactories.GetFactory.GetObjectName(nameof(ReferralId), pluralize: false)}] IS NOT NULL)");

            var now = new DateTimeOffset(2023, 3, 21, 0, 0, 0, TimeSpan.Zero);
            List<ApplicationUser> seedData =
            [
                // Password: @Admin123
                new ApplicationUser { Id = 1, UserName = "admin", PasswordHash = "AQAAAAIAAYagAAAAEMLN3xqYWUja6ShSK0teeCYzziU6b+KghL4AiSXrb03Y3VbBfxKP7LUF3PZAJhQJ+Q==", NormalizedUserName = "ADMIN", Email = "admin@gamaedtech.com", NormalizedEmail = "ADMIN@GAMAEDTECH.COM", EmailConfirmed = true, ConcurrencyStamp = "5BABA139-4AE5-4C47-BC65-DE4849346A17", PhoneNumber = "09355028981", PhoneNumberConfirmed = true, SecurityStamp = "EAF1FA85-3DA1-4A40-90C6-65B97BF903F1", RegistrationDate = now, Enabled = true, Gender = GenderType.Male },
            ];
            _ = builder.HasData(seedData);
        }
    }
}

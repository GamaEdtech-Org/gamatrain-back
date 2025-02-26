namespace GamaEdtech.Common.DataAccess.Context
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections;
    using GamaEdtech.Common.DataAccess;
    using GamaEdtech.Common.DataAccess.Audit;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAccess.ValueConversion;
    using GamaEdtech.Common.DataAccess.ValueGeneration;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using NUlid;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public abstract class IdentityEntityContext<TContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IEntityContext
        where TContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        private readonly string[] shadowProperties = [nameof(IVersionableEntity<TUser, TKey, TKey>.CreationDate), nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUserId), nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyDate), nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)];
        private readonly IHttpContextAccessor httpContextAccessor;

        protected IdentityEntityContext(IServiceProvider serviceProvider)
            : base()
        {
            ServiceProvider = serviceProvider;

            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            ConnectionName = configuration.GetValue<string>("Connection:ConnectionString") + configuration.GetValue<string>("Connection:License");
            DefaultSchema = configuration.GetValue<string>("Connection:DefaultSchema");
            SensitiveDataLoggingEnabled = configuration.GetValue<bool>("Connection:SensitiveDataLoggingEnabled");
            DetailedErrorsEnabled = configuration.GetValue<bool>("Connection:DetailedErrorsEnabled");
            LoggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
            httpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        protected string? ConnectionName { get; }

        protected string? DefaultSchema { get; }

        protected bool SensitiveDataLoggingEnabled { get; }

        protected bool DetailedErrorsEnabled { get; }

        protected ILoggerFactory LoggerFactory { get; }

        protected abstract Assembly EntityAssembly { get; }

        private IServiceProvider ServiceProvider { get; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            PrepareShadowProperties();

            var data = GenerateAudit();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            SaveAudit(data);

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            PrepareShadowProperties();

            var data = GenerateAudit();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await SaveAuditAsync(data);

            return result;
        }

        protected override void OnModelCreating([NotNull] ModelBuilder builder)
        {
            if (!string.IsNullOrEmpty(DefaultSchema))
            {
                _ = builder.HasDefaultSchema(DefaultSchema);
            }

            ConfigureProperties(EntityAssembly.GetTypes());
            ConfigureProperties(typeof(IDataAccess).Assembly.GetTypes());

            _ = builder.ApplyConfigurationsFromAssembly(EntityAssembly);
            _ = builder.ApplyConfigurationsFromAssembly(typeof(IDataAccess).Assembly);

            void ConfigureProperties(Type[] types)
            {
                for (var i = 0; i < types.Length; i++)
                {
                    var type = types[i];
                    var interfaces = type.GetInterfaces();
                    if (!interfaces.Exists(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEntity<,>)))
                    {
                        continue;
                    }

                    if (interfaces.Exists(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IVersionableEntity<,,>)))
                    {
                        _ = builder.Entity(type).HasOne(nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUser))
                                .WithMany().HasForeignKey(nameof(IVersionableEntity<TUser, TKey, TKey>.CreationUserId)).OnDelete(DeleteBehavior.NoAction);

                        _ = builder.Entity(type).HasOne(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUser))
                            .WithMany().HasForeignKey(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)).OnDelete(DeleteBehavior.NoAction);

                        _ = builder.Entity(type).Property(nameof(IVersionableEntity<TUser, TKey, TKey>.LastModifyUserId)).IsRequired(false);
                    }

                    if (type.IsGenericType)
                    {
                        continue;
                    }

                    var properties = type.GetProperties();
                    for (var j = 0; j < properties.Length; j++)
                    {
                        var property = properties[j];

                        if (property.PropertyType == typeof(Guid) && property.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        {
                            _ = builder.Entity(type).Property(property.PropertyType, property.Name).HasDefaultValueSql("NEWSEQUENTIALID()");
                        }
                        else if (property.PropertyType == typeof(Ulid) && property.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        {
                            _ = builder.Entity(type).Property(property.Name).ValueGeneratedOnAdd().HasValueGenerator<UlidGenerator>();
                        }
                    }
                }
            }
        }

        protected override void ConfigureConventions([NotNull] ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            _ = configurationBuilder.Properties<Ulid>().HaveConversion<UlidValueConverter>();
        }

        protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder) => optionsBuilder.EnableSensitiveDataLogging(SensitiveDataLoggingEnabled)
                .EnableDetailedErrors(DetailedErrorsEnabled)
                .UseLoggerFactory(LoggerFactory);

        private static AuditType Convert(EntityState entityState) => entityState switch
        {
            EntityState.Deleted => AuditType.Deleted,
            EntityState.Modified => AuditType.Modified,
            EntityState.Added => AuditType.Added,
            _ => throw new ArgumentOutOfRangeException(nameof(entityState)),
        };

        private static void PrepareAuditIdentifierIds(Audit? audit)
        {
            if (audit?.AuditEntries is null)
            {
                return;
            }

            for (var i = 0; i < audit.AuditEntries.Count; i++)
            {
                var entry = audit.AuditEntries[i];
                if (entry.AuditEntryProperties is null)
                {
                    continue;
                }

                for (var j = 0; j < entry.AuditEntryProperties.Count; j++)
                {
                    var property = entry.AuditEntryProperties[j];
                    if (property.TemporaryProperty is not null)
                    {
                        property.NewValue = property.TemporaryProperty.CurrentValue?.ToString();
                        if (property.TemporaryProperty.Metadata.IsPrimaryKey())
                        {
                            entry.IdentifierId = property.TemporaryProperty.CurrentValue?.ToString();
                        }
                    }
                }
            }
        }

        private void PrepareShadowProperties()
        {
            ChangeTracker.DetectChanges();

            if (httpContextAccessor.HttpContext is null)
            {
                return;
            }

            var entries = ChangeTracker.Entries();
            var i = 0;
            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetInterface(typeof(IVersionableEntity<,,>).Name) is null)
                {
                    continue;
                }

                dynamic versionableEntity = entry.Entity;
                var userId = httpContextAccessor.HttpContext.UserId<TKey>();
                if (entry.State is EntityState.Added)
                {
                    if (userId is not null && !userId.Equals(default))
                    {
                        versionableEntity.CreationUserId = userId;
                    }

                    versionableEntity.CreationDate = DateTimeOffset.Now;
                }
                else if (entry.State is EntityState.Modified)
                {
                    if (userId is not null && !userId.Equals(default))
                    {
                        versionableEntity.LastModifyUserId = userId;
                    }

                    versionableEntity.LastModifyDate = DateTimeOffset.Now;
                }

                i++;
            }
        }

        private Audit? GenerateAudit()
        {
            if (!ServiceProvider.GetRequiredService<IConfiguration>().GetValue<bool>("EnableAudit"))
            {
                return null;
            }

            var audit = new Audit
            {
                Id = Ulid.NewUlid(),
                Date = DateTimeOffset.Now,
                IpAddress = httpContextAccessor.HttpContext.GetClientIpAddress(),
                UserAgent = httpContextAccessor.HttpContext.UserAgent(),
                UserId = httpContextAccessor.HttpContext.UserId<TKey>()?.ToString(),
                UserName = httpContextAccessor.HttpContext?.User?.Identity?.Name,
                AuditEntries = [],
            };
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var auditAttribute = entry.Entity.GetType().GetCustomAttribute<AuditAttribute>();
                if (auditAttribute is null)
                {
                    continue;
                }

                var auditEntry = new AuditEntry
                {
                    Id = Ulid.NewUlid(),
                    AuditId = audit.Id,
                    EntityType = auditAttribute.EntityType,
                    AuditEntryProperties = [],
                    AuditType = Convert(entry.State),
                };

                if (entry.State == EntityState.Added)
                {
                    var ids = entry.Properties.Where(t => t.Metadata.IsPrimaryKey() && !t.IsTemporary).Select(t => t.CurrentValue?.ToString());
                    auditEntry.IdentifierId = ids?.Any() == true ? string.Join(" , ", ids) : null;
                }
                else
                {
                    auditEntry.IdentifierId = entry.State == EntityState.Added ? null : string.Join(" , ", entry.Properties.Where(t => t.Metadata.IsPrimaryKey()).Select(t => t.CurrentValue?.ToString()));
                }

                if (entry.State is not EntityState.Deleted)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (entry.Entity.GetType().GetProperty(property.Metadata.Name)?.GetCustomAttribute<AuditIgnoreAttribute>() is not null)
                        {
                            continue;
                        }

                        if (shadowProperties.Contains(property.Metadata.Name))
                        {
                            continue;
                        }

                        if (entry.State == EntityState.Added || property.OriginalValue?.ToString() != property.CurrentValue?.ToString())
                        {
                            auditEntry.AuditEntryProperties.Add(new AuditEntryProperty
                            {
                                OldValue = entry.State == EntityState.Added ? null : property.OriginalValue?.ToString(),
                                NewValue = property.CurrentValue?.ToString(),
                                PropertyName = property.Metadata.Name,
                                TemporaryProperty = property.IsTemporary ? property : null,
                                Id = Ulid.NewUlid(),
                                AuditEntryId = auditEntry.Id,
                            });
                        }
                    }
                }

                if (entry.State is EntityState.Deleted || auditEntry.AuditEntryProperties.Count > 0)
                {
                    audit.AuditEntries.Add(auditEntry);
                }
            }

            return audit.AuditEntries.Any(t => t.AuditType?.Equals(AuditType.Deleted) == true || t.AuditEntryProperties?.Count > 0) ? audit : null;
        }

        private void SaveAudit(Audit? audit)
        {
            PrepareAuditIdentifierIds(audit);
            if (audit is not null)
            {
                _ = Set<Audit>().Add(audit);
                _ = SaveChanges();
            }
        }

        private async Task SaveAuditAsync(Audit? audit)
        {
            PrepareAuditIdentifierIds(audit);
            if (audit is not null)
            {
                _ = await Set<Audit>().AddAsync(audit);
                _ = await SaveChangesAsync();
            }
        }
    }
}

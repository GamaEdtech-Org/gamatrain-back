namespace GamaEdtech.Backend.Infrastructure.EntityFramework.Context
{
    using GamaEdtech.Backend.Data.Entity.Identity;

    using Farsica.Framework.DataAnnotation;

    using global::EntityFramework.Exceptions.SqlServer;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [ServiceLifetime(ServiceLifetime.Transient, "System.IServiceProvider,System.ComponentModel")]
    public class ApplicationDBContext(IServiceProvider serviceProvider) : Farsica.Framework.DataAccess.Context.IdentityEntityContext<ApplicationDBContext, ApplicationUser, ApplicationRole,
        int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>(serviceProvider)
    {
        protected override Assembly EntityAssembly => typeof(ApplicationUser).Assembly;

        protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            _ = optionsBuilder.UseSqlServer(ConnectionName, t => t.UseNetTopologySuite());
            _ = optionsBuilder.UseExceptionProcessor();
        }
    }
}

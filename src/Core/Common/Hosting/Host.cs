namespace GamaEdtech.Common.Hosting
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using Destructurama;

    using GamaEdtech.Common.Core;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Serilog;
    using GamaEdtech.Common.Startup;
    using GamaEdtech.Common.DataAccess.Context;
    using GamaEdtech.Common.Logging;
    using GamaEdtech.Common.Data;

    public static class Host
    {
        public static async Task RunAsync<TStartup>(string[] args)
            where TStartup : class => await RunInternalAsync<TStartup>(args, false);

        public static async Task RunAsync<TStartup, TUser, TRole>(string[] args)
            where TStartup : Startup<TUser, TRole>
            where TUser : class
            where TRole : class => await RunInternalAsync<TStartup>(args, true);

        public static IHost? CreateHost<TStartup>(string[] args)
            where TStartup : class
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Globals.ProviderType = config.GetValue<DbProviderType>("Connection:ProviderType");
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Destructure.UsingAttributes()
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .CreateLogger();

            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    _ = logging.ClearProviders();
                    _ = logging.AddFilelog();
                })
                .ConfigureWebHostDefaults(webBuilder => _ = webBuilder.UseStartup<TStartup>()
                    .UseSetting(WebHostDefaults.ApplicationKey, typeof(TStartup).GetTypeInfo().Assembly.FullName)
                    .UseKestrel(options => options.AddServerHeader = false)
                    .UseIIS()).Build();
        }

        private static async Task RunInternalAsync<TStartup>(string[] args, bool checkMigration)
            where TStartup : class
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var host = CreateHost<TStartup>(args);
#pragma warning restore CA2000 // Dispose objects before losing scope
            if (host is null)
            {
                return;
            }

            if (checkMigration)
            {
                using var scope = host.Services.CreateScope();
                if (scope.ServiceProvider.GetService(typeof(IEntityContext)) is DbContext context)
                {
                    await context.Database.MigrateAsync();
                }
            }

            await host.RunAsync();
        }
    }
}

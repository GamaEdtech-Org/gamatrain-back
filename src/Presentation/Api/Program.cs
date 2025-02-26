namespace GamaEdtech.Presentation.Api
{
    using GamaEdtech.Domain.Entity.Identity;

    using System.Threading.Tasks;

    public static class Program
    {
        public static async Task Main(string[] args) => await Common.Hosting.Host.RunAsync<Startup, ApplicationUser, ApplicationRole>(args);
    }
}

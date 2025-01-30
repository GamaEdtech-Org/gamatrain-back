namespace GamaEdtech.Backend.UI.Web
{
    using GamaEdtech.Backend.Data.Entity.Identity;

    using System.Threading.Tasks;

    public static class Program
    {
        public static async Task Main(string[] args) => await Farsica.Framework.Hosting.Host.RunAsync<Startup, ApplicationUser, ApplicationRole>(args);
    }
}

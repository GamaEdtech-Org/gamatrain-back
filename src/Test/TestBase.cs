namespace GamaEdtech.Test
{
    using GamaEdtech.Presentation.Api;

    public abstract class TestBase
    {
        protected static Lazy<IServiceProvider?> Services => Startup.Services;

        protected TestBase() => Environment.SetEnvironmentVariable("Test", "True");
    }
}

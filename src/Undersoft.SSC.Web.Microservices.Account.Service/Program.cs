using NLog.Web;
using Undersoft.SDK.Service.Configuration;

namespace Undersoft.SSC.Web.Microservices.Account.Service
{
    public class Program
    {
        static string[]? _args;
        static IWebHost? _webapi;

        public static void Main(string[] args)
        {
            _args = args;
            Launch();
        }

        static IWebHost Build()
        {
            var builder = new WebHostBuilder();

            builder.Info<Runlog>("Starting SSC Service API ....");

            _webapi = builder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(ServiceConfigurationHelper.BuildConfiguration())
                .UseKestrel()
                .ConfigureKestrel((c, o) => o
                    .Configure(c.Configuration
                    .GetSection("Kestrel")))
                .UseStartup<Startup>()
                .UseNLog()
                .Build();

            return _webapi;
        }

        public static void Launch()
        {
            try
            {                
                Build().Run();
            }
            catch (Exception exception)
            {
                Log.Error<Runlog>(null, "SSC Service API terminated unexpectedly ....", exception);
            }
            finally
            {
                Log.Info<Runlog>(null, "SSC Service API shutted down ....");
            }
        }

        public static async Task Restart()
        {
            Log.Info<Runlog>(null, "Restarting SSC Service API ....");

            Task.WaitAll(Shutdown());

            await Task.Run(() => Launch());
        }

        public static async Task Shutdown()
        {
            Log.Info<Runlog>(null, "Shutting down SSC Service API ....");

            _webapi.Info<Runlog>("Stopping SSC Service API ....");

            if(_webapi != null)
                await _webapi.StopAsync(TimeSpan.FromSeconds(5));
        }
    }
}
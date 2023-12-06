using Undersoft.SDK.Service.Application;
using Undersoft.SDK.Service.Application.DataServer;
using Undersoft.SSC.Infrastructure.Persistance.Stores;

namespace Undersoft.SSC.Web.Microservices.Schedule.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddApplicationSetup()
                .ConfigureApplication(true, AppDomain.CurrentDomain.GetAssemblies())
                .AddIdentityService<ServiceIdentityStore>()
                .AddDataServer<IDataServiceStore>(
                    DataServerTypes.All,
                    builder =>
                        builder
                            .AddDataServices<ServiceEntryStore>()
                            .AddDataServices<ServiceReportStore>()
                            .AddDataServices<ServiceIdentityStore>()
                            .AddDataServices<ServiceEventStore>()
                            .AddIdentityActionSet()
                );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseInternalProvider()
               .UseApplicationSetup(env)
               .UseDataMigrations();
        }
    }
}

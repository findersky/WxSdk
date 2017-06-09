using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pioneer.WxSdk.Message;

namespace Pioneer.WxSdk.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            SdkSetup.RegisterListener(new DefaultMessageListener());

            SdkSetup.MessageTokenGetter = (s) => { return new PublicAccount() { MessageToken = "test" }; };

            SdkSetup.RefreshPublicAccountInfo = (pa) => { };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddEAPService();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseEAPService();


        }
    }
}

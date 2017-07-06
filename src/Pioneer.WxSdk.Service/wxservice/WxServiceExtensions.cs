using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

 namespace  Pioneer.WxSdk.Service
 {
     public static class EAPServiceExtensions
    {
        /// <summary>
        /// Enable directory browsing on the current path
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseEAPService(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<EAPServiceMiddleware>();
        }

        public static IServiceCollection AddEAPService(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            
            services.AddSingleton<IWxService>(new WxService());

            SdkSetup.RegisterListener(new DefaultMessageListener());

            SdkSetup.MessageTokenGetter = (s) => { return new PublicAccount() { MessageToken = "test" }; };

            SdkSetup.RefreshPublicAccountInfo = (pa) => { };


            services.AddLogging();

            return services;
        }
    }
 }
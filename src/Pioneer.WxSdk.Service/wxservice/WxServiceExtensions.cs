using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pioneer.WxSdk.Message;
using System;

namespace Pioneer.WxSdk.Service
{
    public static class WxServiceExtensions
    {
        /// <summary>
        /// Enable directory browsing on the current path
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWxService(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<EAPServiceMiddleware>();
        }

        public static IServiceCollection AddWxService(this IServiceCollection services)
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
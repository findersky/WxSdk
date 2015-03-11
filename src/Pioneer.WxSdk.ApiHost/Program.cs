using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Netsharp.Core;
using Topshelf;
using Netsharp.TopshelfLog;

namespace Pioneer.WxSdk.ApiHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Netsharp.Core.LogFactory.Config("log", ApiConfig.Instance.LogLevel);


            var host = HostFactory.New(x =>
            {
                x.Service<ApiService>();
                x.RunAsLocalSystem();
                x.UseNLog();
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription(ApiConfig.Instance.ServiceDescription);
                x.SetDisplayName(ApiConfig.Instance.ServiceDisplayName);
                x.SetServiceName(ApiConfig.Instance.ServiceName);

            });
            host.Run();
        }
    }
}

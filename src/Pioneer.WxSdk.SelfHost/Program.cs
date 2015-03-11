using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Netsharp.Core;
using Topshelf;
using Netsharp.TopshelfLog;

namespace Pioneer.WxSdk.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Netsharp.Core.LogFactory.Config("log", TestConfig.Instance.LogLevel);


            var host = HostFactory.New(x =>
            {
                x.Service<SdkService>();
                x.RunAsLocalSystem();
                x.UseNLog();
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription("SDKTest");
                x.SetDisplayName("SDKTest");
                x.SetServiceName("SDKTest");

            });
            host.Run();


        }
    }



}

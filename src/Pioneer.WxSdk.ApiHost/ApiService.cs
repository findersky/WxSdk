using Microsoft.Owin.Hosting;
using Netsharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Owin;

namespace Pioneer.WxSdk.ApiHost
{
    public class ApiService : ServiceControl, ServiceShutdown
    {
        INLogger logger = Netsharp.Core.LogFactory.GetLogger(typeof(ApiService));

        IDisposable innerHost;

        public bool Start(HostControl hostControl)
        {

            if (string.IsNullOrEmpty(ApiConfig.Instance.ServerUrl))
            {
                throw new Exception("Url为空 无法启动服务器");
            }


            StartOptions so = new StartOptions();
            so.Urls.Add(ApiConfig.Instance.ServerUrl);

            innerHost = WebApp.Start(so, builder =>
            {
                ApiModel.Init();

                builder.Use<ApiModel>();
                builder.Use<ApiInfoModel>();
            });

            logger.Info("监听地址:" + ApiConfig.Instance.ServerUrl);


            logger.Info("启动成功");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {

            logger.Info("服务停止");

            return true;
        }

        public void Shutdown(HostControl hostControl)
        {

            logger.Info("服务关闭");
            innerHost.Dispose();
        }
    }
}

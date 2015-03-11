using Microsoft.Owin.Hosting;
using Netsharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Owin;

namespace Pioneer.WxSdk.SelfHost
{
    public class SdkService : ServiceControl, ServiceShutdown
    {
        INLogger logger = Netsharp.Core.LogFactory.GetLogger(typeof(SdkService));

        IDisposable innerHost;

        public bool Start(HostControl hostControl)
        {

            if (string.IsNullOrEmpty(TestConfig.Instance.ServerUrl))
            {
                throw new Exception("Url为空 无法启动服务器");
            }


            Pioneer.WxSdk.SdkSetup.MessageTokenGetter = (dic) =>
            {

                PublicAccount pa = new PublicAccount();

                pa.EncryptionKey = TestConfig.Instance.EncryptionKey;
                pa.MessageToken = TestConfig.Instance.Token;
                pa.AppId = TestConfig.Instance.AppId;
                return pa;
            };

            SdkSetup.RegisterListener(new WxSdk.Message.DefaultMessageListener());


            StartOptions so = new StartOptions();
            so.Urls.Add(TestConfig.Instance.ServerUrl);

            innerHost = WebApp.Start(so, builder =>
            {
                builder.Use(new MessageModel().ProcessRequest);
            });

            logger.Info("监听地址:" + TestConfig.Instance.ServerUrl);


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

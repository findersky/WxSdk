using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.WxSdk
{
    public enum Deploy
    {
        Client, Server
    }

    public class SdkConfig
    {

        public string RemoteServer
        {
            get;
            set;
        }

        public Deploy Deploy
        {
            get;
            set;
        }

        public string ListenerType
        {
            get;
            set;
        }

        static SdkConfig _instance;

        public static SdkConfig Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                string location = typeof(SdkConfig).Assembly.Location;

                _instance = JsonService.FromFile<SdkConfig>(Path.Combine(location, "config", "sdkconfig.json"), Encoding.Default, true);

                if (_instance == null)
                {
                    _instance = new SdkConfig();
                    _instance.RemoteServer = "http://127.0.0.1:15127";
                    _instance.Deploy = Deploy.Client;
                }

                return _instance;
            }
        }

    }
}

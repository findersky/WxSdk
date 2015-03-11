using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.WxSdk.ApiHost
{
    public class ApiConfig
    {

        public string ServerUrl
        {
            get;
            set;
        }

        public int LogLevel
        {
            get;
            set;
        }

        public string ServiceName
        {
            get;
            set;
        }

        public string ServiceDisplayName
        {
            get;
            set;
        }

        public string ServiceDescription
        {
            get;
            set;
        }

        static ApiConfig _instance;

        public static ApiConfig Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FileService.FromFile<ApiConfig>(Path.Combine(FileService.BinPath, "config", "apiconfig.json"), null, SerializeType.Json, true);

                return _instance;
            }
        }

    }
}

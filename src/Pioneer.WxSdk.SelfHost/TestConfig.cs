using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.WxSdk.SelfHost
{
    public class TestConfig
    {
        public string ServerUrl
        {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }

        public string EncryptionKey
        {
            get;
            set;
        }

        public string AppId
        {
            get;
            set;
        }

        public string Appsecret
        {
            get;
            set;
        }

        public int LogLevel
        {
            get;
            set;
        }

        static TestConfig _instance;

        public static TestConfig Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FileService.FromFile<TestConfig>(Path.Combine(FileService.BinPath, "testconfig.json"), null, SerializeType.Json, true);

                return _instance;
            }
        }
    }
}

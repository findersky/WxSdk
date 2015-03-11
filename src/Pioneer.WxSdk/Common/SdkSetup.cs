using Pioneer.WxSdk.Api;
using Pioneer.WxSdk.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{


    public static class SdkSetup
    {
        public static Func<IDictionary<string, string>, PublicAccount> MessageTokenGetter;

#if !Pioneer && !Jaguar
        public static Func<string, ILog> LogProvider;
#endif

        public static Action<PublicAccount> RefreshPublicAccountInfo;


        public static void RegisterListener(IMessageListener listerer)
        {
            MessageCenter.RegisterListener(listerer);
        }

    }
}

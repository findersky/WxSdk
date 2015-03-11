using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    class AccessTokenFactory
    {
        static ILog logger = LogFactory.GetLogger<AccessTokenFactory>();


        internal static void Replace(PublicAccount ato)
        {
            tokens[ato.OriginalId] = ato;
        }

        public static PublicAccount RegetAccessToken(PublicAccount ato)
        {
            AccessTokenRequest atr = new AccessTokenRequest();
            atr.PublicAccountInfo = ato;

            var ret = atr.GetResponse();

            ato.LastUpdateTime = DateTime.Now;
            ato.AccessToken = ret.Access_token;

            tokens[ato.OriginalId] = ato;

            return ato;

        }

        public static PublicAccount Refresh(PublicAccount ato)
        {
            PublicAccount tmpAto = null;

            if (tokens.TryGetValue(ato.OriginalId, out tmpAto))
            {
                if (!tmpAto.IsExpired)
                {
                    return tmpAto;
                }
            }

            return RegetAccessToken(ato);
        }



        static ConcurrentDictionary<string, PublicAccount> tokens = new ConcurrentDictionary<string, PublicAccount>();
    }
}

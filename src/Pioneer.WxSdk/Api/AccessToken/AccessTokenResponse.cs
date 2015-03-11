using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class AccessTokenResponse : Response
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string Access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int Expires_in { get; set; }

    }
}

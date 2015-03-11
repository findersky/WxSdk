using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("查询自定义菜单", "/api/MenuQuery")]
    public class MenuQueryRequest : Request<MenuQueryResponse>
    {
        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", this.AccessToken); }
        }

        protected override MenuQueryResponse DoResponse()
        {
            return this.HttpGet();
        }
    }
}

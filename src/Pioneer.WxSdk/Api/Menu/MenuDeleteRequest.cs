using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("删除自定义菜单", "/api/MenuDelete")]
    public class MenuDeleteRequest : Request<Response>
    {
        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", this.AccessToken); }
        }


        protected override Response DoResponse()
        {
            return this.HttpGet();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("查询所有分组", "/api/GroupQuery")]
    public class GroupQueryRequest : Request<GroupQueryResponse>
    {

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}", this.AccessToken); }
        }

        protected override GroupQueryResponse DoResponse()
        {
            return this.HttpGet();
        }
    }
}

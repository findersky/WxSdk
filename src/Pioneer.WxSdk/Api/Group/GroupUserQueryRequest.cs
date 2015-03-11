using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("查询用户所在分组", "/api/GroupUserQuery")]
    public class GroupUserQueryRequest : Request<GroupUserQueryResponse>
    {
        public string OpenId
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.OpenId))
                return Tuple.Create(false, "OpenId is null");

            return base.DoValidate();
        }

        protected override GroupUserQueryResponse DoResponse()
        {
            var data = new
            {
                openid = this.OpenId
            };

            var json = this.Serialize(data);

            return this.HttpPost(json);
        }
    }
}

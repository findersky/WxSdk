using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("移动用户分组", "/api/GroupUserMove")]
    public class GroupUserMoveRequest : Request<Response>
    {

        public string OpenId
        {
            get;
            set;
        }

        public string ToGroupId
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.OpenId))
                return Tuple.Create(false, "OpenId is null");

            if (string.IsNullOrEmpty(this.ToGroupId))
            {
                return Tuple.Create(false, "ToGroupId is null");
            }

            return base.DoValidate();
        }

        protected override Response DoResponse()
        {
            var data = new
            {
                openid = this.OpenId,
                to_groupid = this.ToGroupId
            };

            string json = this.Serialize(data);

            return this.HttpPost(json);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("创建用户分组", "/api/GroupCreate")]
    public class GroupCreateRequest : Request<CreateGroupResponse>
    {
        public string GroupName
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {

            if (string.IsNullOrEmpty(GroupName))
            {
                return Tuple.Create(false, "GroupName is null");
            }

            return Tuple.Create(true, string.Empty);
        }

        protected override CreateGroupResponse DoResponse()
        {
            var data = new
            {
                group = new
                {
                    name = this.GroupName
                }
            };

            string json = this.Serialize(data);

            return this.HttpPost(json);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{

    [Apipath("修改组名称", "/api/GroupModify")]
    public class GroupModifyRequest : Request<Response>
    {
        public string GroupId
        {
            get;
            set;
        }

        public string GroupName
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}", this.AccessToken); }
        }


        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(GroupId))
                return Tuple.Create(false, "GroupId is null");

            if (string.IsNullOrEmpty(GroupName))
            {
                return Tuple.Create(false, "GroupName is null");
            }

            return base.DoValidate();
        }

        protected override Response DoResponse()
        {
            var data = new
            {
                group = new
                {
                    id = this.GroupId,
                    name = this.GroupName
                }
            };

            string json = this.Serialize(data);

            return this.HttpPost(json);

        }
    }
}

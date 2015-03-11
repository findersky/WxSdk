using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class MarkUserRequest : Request<Response>
    {
        public string OpenId
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(OpenId))
            {
                return Tuple.Create(false, "Openid is null");
            }

            if (string.IsNullOrEmpty(this.Remark))
            {
                return Tuple.Create(false, "Remark is null");
            }

            if (this.Remark.Length >= 30)
            {
                if (string.IsNullOrEmpty(this.Remark))
                {
                    return Tuple.Create(false, "Remark must be less than 30 characters");
                }
            }

            return base.DoValidate();
        }

        protected override Response DoResponse()
        {
            var o = new
            {
                openid = this.OpenId,
                remark = this.Remark
            };

            string json = this.Serialize(o);

            return this.HttpPost(json);
        }
    }
}

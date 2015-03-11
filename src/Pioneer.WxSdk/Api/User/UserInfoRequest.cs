using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("获取用户信息", "/api/UserInfo")]
    public class UserInfoRequest : Request<UserInfoResponse>
    {

        public string OpenId
        {
            get;
            set;
        }

        public string Lang
        {
            get
            {
                return "zh_CN";
            }
        }

        public override string Url
        {
            get
            {
                return string.Format("http://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}", this.AccessToken, this.OpenId, this.Lang);
            }
        }

        protected override Tuple<bool, string> DoValidate()
        {

            if (string.IsNullOrEmpty(this.OpenId))
            {
                return Tuple.Create(false, "OpenId is null ");
            }

            return base.DoValidate();
        }

        protected override UserInfoResponse DoResponse()
        {
            return this.HttpGet();
        }
    }
}

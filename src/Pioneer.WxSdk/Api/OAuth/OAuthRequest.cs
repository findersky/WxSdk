using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{

    /// <summary>
    /// 此过程需要两次调用
    /// 1  设置参数后  调用GetAuthUrl 用户确认操作后 跳转到RedirectUrl
    /// 在 RedirectUrl请求中重新构造当前对象  并取得Url中的Code参数  再调用 GerResponse得到信息
    /// </summary>
    [Apipath("OAuth授权", "/api/OAuth")]
    public class OAuthRequest : Request<OAuthResponse>
    {
        public string AppId
        {
            get;
            set;
        }

        public string AppSecret
        {
            get;
            set;
        }

        public string GrantType
        {
            get
            {
                return "authorization_code";
            }
        }

        public string ResponseType
        {
            get
            {
                return "code";
            }
        }

        public bool? OnlyGetOpenId
        {
            get;
            set;
        }

        string Scope
        {
            get
            {
                if (OnlyGetOpenId == true)
                {
                    return "snsapi_base";
                }
                else
                {
                    return "snsapi_userinfo";
                }
            }
        }

        public string Code
        {
            get;
            set;
        }

        public string RedirectUrl
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}", this.AppId, this.AppSecret, this.Code, this.GrantType); }
        }

        public override Tuple<bool, string> Validate()
        {
            if (string.IsNullOrEmpty(this.AppId))
                return Tuple.Create(false, "AppId is null");

            if (string.IsNullOrEmpty(this.AppSecret))
                return Tuple.Create(false, "AppSecret is null");

            if (string.IsNullOrEmpty(this.Code))
                return Tuple.Create(false, " Code is null");

            return base.DoValidate();
        }

        public override OAuthResponse GetResponse()
        {
            Tuple<bool, string> valudateResult = this.Validate();

            if (!valudateResult.Item1)
                throw new WxException(valudateResult.Item2);

            return DoResponse();
        }

        protected override OAuthResponse DoResponse()
        {
            OAuthResponse or = this.HttpGet();

            if (or.Scope == "snsapi_base")
            {
                return or;
            }

            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", or.Access_token, or.Openid);

            using (HttpWebClient client = new HttpWebClient())
            {
                string json = client.DownloadString(url);

                return this.Deserialize(json);
            }

        }

        public string GetAuthUrl()
        {
            if (string.IsNullOrEmpty(this.AppId))
            {
                throw new ArgumentNullException("appid");
            }

            if (string.IsNullOrEmpty(this.RedirectUrl))
            {
                throw new ArgumentNullException("RedirectUrl");
            }

            if (OnlyGetOpenId == null)
                throw new ArgumentNullException("OnlyGetOpenId");

            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect",
                    this.AppId, HttpWebClient.UrlEncode(this.RedirectUrl), this.ResponseType, this.Scope, state);
            return url;
        }


    }
}

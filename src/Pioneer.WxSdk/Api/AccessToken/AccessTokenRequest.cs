using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("获取AccessToken", "/api/AccessToken")]
    public class AccessTokenRequest : Request<AccessTokenResponse>
    {
        protected string AppId
        {
            get
            {
                if (this.PublicAccountInfo == null)
                {
                    return null;
                }
                return this.PublicAccountInfo.AppId;
            }
        }

        protected string AppSecret
        {
            get
            {
                if (this.PublicAccountInfo == null)
                    return null;
                return this.PublicAccountInfo.AppSecret;
            }

        }


        public string GrantType
        {
            get
            {
                return "client_credential";
            }
        }

        public override string Url
        {
            get
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}", this.GrantType, this.AppId, this.AppSecret);
                return url;
            }
        }

        public override Tuple<bool, string> Validate()
        {
            if (string.IsNullOrEmpty(this.AppId))
                return Tuple.Create(false, "AppId is null");


            if (string.IsNullOrEmpty(this.AppSecret))
                return Tuple.Create(false, "AppSecret is null");

            return Tuple.Create(true, string.Empty);
        }

        public override AccessTokenResponse GetResponse()
        {

            Tuple<bool, string> valudateResult = this.Validate();

            if (!valudateResult.Item1)
                throw new WxException(valudateResult.Item2);

            return DoResponse();
        }
        


        protected override AccessTokenResponse DoResponse()
        {
            using (HttpWebClient client = new HttpWebClient())
            {
                string json = client.DownloadString(this.Url);

                this.logger.Trace(json);

                AccessTokenResponse o = null;

                try
                {
                    o = this.Deserialize(json);
                }
                catch (WxException we)
                {

                    this.logger.Error(we);

                    if (SdkSetup.RefreshPublicAccountInfo != null)
                    {
                        SdkSetup.RefreshPublicAccountInfo(this.PublicAccountInfo);
                    }
                    else
                    {
                        throw;
                    }
                    AccessTokenFactory.Replace(this.PublicAccountInfo);

                    json = client.DownloadString(this.Url);
                    try
                    {
                        o = this.Deserialize(json);
                    }
                    catch (WxException we2)
                    {
                        this.logger.Error(we2);
                        this.logger.Error(this.AppId);
                        this.logger.Error(this.AppSecret);

                        throw;
                    }
                }

                return o;
            }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            throw new NotImplementedException();
        }
    }
}

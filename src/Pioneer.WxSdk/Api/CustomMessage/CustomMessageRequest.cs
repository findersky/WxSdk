using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    /// <summary>
    /// 客服消息
    /// </summary>
    public abstract class CustomMessageRequest : Request<Response>
    {
        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", this.AccessToken); }
        }

        public string OpenId
        {
            get;
            set;
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(OpenId))
                return Tuple.Create(false, "Openid is null");

            return base.DoValidate();
        }


        protected override Response DoResponse()
        {
            var o = GetMessage();

            string json = this.Serialize(o);

            return this.HttpPost(json);

        }

        protected abstract object GetMessage();

    }
}

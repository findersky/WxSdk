using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class ShortUrlRequest : Request<ShortUrlResponse>
    {
        public string Action
        {
            get
            {
                return "long2short";
            }
        }

        public string LongUrl
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(LongUrl))
            {
                return Tuple.Create(false, "LongUrl is null");
            }

            return base.DoValidate();
        }

        protected override ShortUrlResponse DoResponse()
        {
            var o = new
            {
                action = this.Action,
                long_url = this.LongUrl
            };

            string json = this.Serialize(o);

            return this.HttpPost(json);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class UpdateFeedbackRequest : Request<Response>
    {
        public string OpenId
        {
            get;
            set;
        }

        public string FeedBackId
        {
            get;
            set;
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.OpenId))
                throw new ArgumentNullException("OpenId is null");

            if (string.IsNullOrEmpty(this.FeedBackId))
                throw new ArgumentNullException("FeedBackId is null");

            return base.DoValidate();
        }


        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/payfeedback/update?access_token={0}&openid={1}&feedbackid={2}", this.AccessToken, this.OpenId, this.FeedBackId); }
        }

        protected override Response DoResponse()
        {
            return this.HttpGet();
        }
    }
}

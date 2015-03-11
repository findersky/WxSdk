using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class DeletePushMessageRequest : Request<Response>
    {

        public string Messageid
        {
            get;
            set;
        }


        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(Messageid))
                return Tuple.Create(false, "MessageId is null");

            return base.DoValidate();
        }

        protected override Response DoResponse()
        {
            var o = new { msg_id = this.Messageid };

            string json = this.Serialize(o);

            return this.HttpPost(json);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("发送客服消息,语音", "/api/VoiceCustomerMessage")]
    public class VoiceCustomerMessageRequest : CustomMessageRequest
    {
        public string MediaId
        {
            get;
            set;
        }

        protected override object GetMessage()
        {
            return new
            {
                touser = this.OpenId,
                msgtype = "voice",
                voice = new
                {
                    media_id = this.MediaId
                }
            };

        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.MediaId))
                return Tuple.Create(false, "MediaId is null");

            return base.DoValidate();
        }
    }
}

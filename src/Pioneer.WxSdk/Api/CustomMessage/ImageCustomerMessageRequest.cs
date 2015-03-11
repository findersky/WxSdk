using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("发送客服消息,图片", "/api/ImageCustomerMessage")]
    public class ImageCustomerMessageRequest : CustomMessageRequest
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
                touser = OpenId,
                msgtype = "image",
                image = new
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

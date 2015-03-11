using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("发送客服消息,文本", "/api/TextCustomerMessage")]
    public class TextCustomerMessageRequest : CustomMessageRequest
    {
        public string Content
        {
            get;
            set;
        }

        protected override object GetMessage()
        {
            return new
            {
                touser = this.OpenId,
                msgtype = "text",
                text = new
                {
                    content = this.Content
                }
            };
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.Content))
                return Tuple.Create(false, "Content is null");

            return base.DoValidate();
        }
    }
}

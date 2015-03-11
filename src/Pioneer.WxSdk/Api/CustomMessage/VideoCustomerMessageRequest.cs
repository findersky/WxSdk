using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("发送客服消息,视频", "/api/VideoCustomerMessage")]
    public class VideoCustomerMessageRequest : CustomMessageRequest
    {
        public string MediaId
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        protected override object GetMessage()
        {
            return new
            {
                touser = this.OpenId,
                msgtype = "video",
                video = new
                {
                    media_id = this.MediaId,
                    description = this.Description,
                    title = this.Title
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    /// <summary>
    /// 群发消息预览
    /// </summary>
    public class PreviewMessageRequest : Request<MessagePreviewResponse>
    {

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token={0}", this.AccessToken); }
        }

        public string MessageType
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public string MediaId
        {
            get;
            set;
        }

        public string OpenId
        {
            get;
            set;
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (MessageType == PushMessageType.Text)
            {
                if (string.IsNullOrEmpty(this.Content))
                {
                    return Tuple.Create(false, "Content is null");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.MediaId))
                {
                    return Tuple.Create(false, "MediaId is null");
                }
            }

            if (string.IsNullOrEmpty(OpenId))
            {
                return Tuple.Create(false, "Openid  is null");
            }
            return base.DoValidate();
        }

        protected override MessagePreviewResponse DoResponse()
        {
            dynamic o = GetMessageObject();

            string json = this.Serialize(o);

            return this.HttpPost(json);
        }

        protected virtual dynamic GetMessageObject()
        {
            dynamic jsonData = new System.Dynamic.ExpandoObject();

            if (this.MessageType == PushMessageType.Text)
            {
                jsonData.text = new { content = this.Content };
                jsonData.msgtype = "text";

            }
            else if (this.MessageType == PushMessageType.News)
            {
                jsonData.mpnews = new { media_id = this.MediaId };
                jsonData.msgtype = "mpnews";
            }
            else if (this.MessageType == PushMessageType.Image)
            {

                jsonData.image = new { media_id = this.MediaId };
                jsonData.msgtype = "image";
            }
            else if (this.MessageType == PushMessageType.Voice)
            {
                jsonData.voice = new { media_id = this.MediaId };
                jsonData.msgtype = "voice";
            }
            else
            {
                throw new WxException("NotSupport MessageType");
            }

            jsonData.touser = this.OpenId;

            return jsonData;
        }
    }
}

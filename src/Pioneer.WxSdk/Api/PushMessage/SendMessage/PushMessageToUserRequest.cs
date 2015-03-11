using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    /// <summary>
    /// 按openId 群发
    /// </summary>
    public class PushMessageToUserRequest : Request<PushMessageResponse>
    {
        public PushMessageToUserRequest()
        {
            this.OpenIds = new List<string>();
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

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}", this.AccessToken); }
        }

        public List<string> OpenIds
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

            if (this.OpenIds.Count == 0)
            {
                return Tuple.Create(false, "Openids.count=0");
            }
            return base.DoValidate();
        }

        protected override PushMessageResponse DoResponse()
        {
            dynamic o = GetMessageObject();

            string json = this.Serialize(o);

            return this.HttpPost(json);
        }

        protected virtual dynamic GetMessageObject()
        {
            dynamic jsonData = new ExpandoObject();

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

            jsonData.touser = this.OpenIds;

            return jsonData;
        }
    }


    [Apipath("发送消息到用户.视频", "/api/VideoMessageToUser")]
    /// <summary>
    /// 这里的Media直接通过上传下载多媒体文件得到  不再需要通过上传视频接口得到
    /// </summary>
    public class VideoMessageToUserRequest : PushMessageToUserRequest
    {

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.MediaId))
            {
                return Tuple.Create(false, "MediaId is null");
            }

            return base.DoValidate();
        }

        protected override object GetMessageObject()
        {
            var jsonData = new
            {
                touser = this.OpenIds,
                mpvideo = new { media_id = GetMediaId() },
                msgtype = "mpvideo"
            };

            return jsonData;
        }

        string GetMediaId()
        {
            UploadVideoRequest uvr = new UploadVideoRequest();
            uvr.MediaId = this.MediaId;
            uvr.PublicAccountInfo = this.PublicAccountInfo;
            uvr.Title = this.Title;
            uvr.Description = this.Description;

            var rsp = uvr.GetResponse();

            return rsp.Media_id;
        }
    }
}

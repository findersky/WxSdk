using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    /// <summary>
    /// 按组群发
    /// </summary>
    public class PushMessageToGroupRequest : Request<PushMessageResponse>
    {
        public PushMessageToGroupRequest()
        {
            this.IsToAll = true;
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

        public bool IsToAll
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}", this.AccessToken); }
        }

        public string GroupId
        {
            get;
            set;
        }

        public string MediaId
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

            if (!IsToAll)
            {
                if (string.IsNullOrEmpty(this.GroupId))
                {
                    return Tuple.Create(false, "GroupId is null");
                }
            }
            return base.DoValidate();
        }

        protected override PushMessageResponse DoResponse()
        {
            var o = GetMessageObject();
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

            jsonData.filter = new { group_id = this.GroupId, is_to_all = this.IsToAll };

            return jsonData;
        }
    }

    [Apipath("发送消息到用户组.视频", "/api/VideoMessageToGroup")]
    public class VideoMessageToGroupRequest : PushMessageToGroupRequest
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
                filter = new { group_id = this.GroupId, is_to_all = this.IsToAll },
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{

    [Apipath("发送客服消息,音乐", "/api/MusicCustomerMessage")]
    public class MusicCustomerMessageRequest : CustomMessageRequest
    {
        /// <summary>
        /// 缩略图的媒体ID 
        /// </summary>
        public string ThumbMediaId
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

        /// <summary>
        /// 音乐链接 
        /// </summary>
        public string MusicUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 高品质音乐链接，wifi环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl
        {
            get;
            set;
        }

        protected override object GetMessage()
        {
            return new
            {
                touser = this.OpenId,
                msgtype = "music",
                music = new
                {
                    title = this.Title,
                    description = this.Description,
                    musicurl = this.MusicUrl,
                    hqmusicurl = this.HQMusicUrl,
                    thumb_media_id = this.ThumbMediaId
                }
            };

        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.MusicUrl))
                return Tuple.Create(false, "MusicUrl is null");

            if (string.IsNullOrEmpty(this.HQMusicUrl))
                return Tuple.Create(false, "HQMusicUrl is null");

            if (string.IsNullOrEmpty(this.ThumbMediaId))
            {
                return Tuple.Create(false, "ThumbMediaId");
            }

            return base.DoValidate();
        }
    }
}

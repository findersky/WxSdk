using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("上传视频 （用于群发视频消息", "/api/UploadVideo")]
    public class UploadVideoRequest : Request<UpLoadNewsResponse>
    {
        public string MediaId
        {
            get;
            set;
        }

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

        public override string Url
        {
            get { return string.Format("https://file.api.weixin.qq.com/cgi-bin/media/uploadvideo?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(this.MediaId))
                Tuple.Create(false, "Mediaid is null");

            if (string.IsNullOrEmpty(this.Title))
                return Tuple.Create(false, "Title is null");

            return base.DoValidate();
        }

        protected override UpLoadNewsResponse DoResponse()
        {
            var jsonobject = new
            {

                media_id = this.MediaId,
                title = this.Title,
                description = this.Description
            };

            return this.HttpPost(this.Serialize(jsonobject));
        }
    }
}

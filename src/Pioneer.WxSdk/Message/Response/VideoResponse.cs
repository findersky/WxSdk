using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    public class VideoResponse : ResponseMessage
    {
        public VideoResponse()
            : base("video")
        {

        }

        public VideoResponse(RequestMessage message)
            : base("video", message)
        {

        }

        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        protected override string InnerToxml()
        {
            string baseXml = base.InnerToxml();

            return baseXml + @"<Video><MediaId>" + this.MediaId + "</MediaId><Title>" + this.Title + "</Title><Description>" + this.Description + "</Description></Video> ";
        }
    }
}

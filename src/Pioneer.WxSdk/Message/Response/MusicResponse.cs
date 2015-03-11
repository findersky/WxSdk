using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    public class MusicResponse : ResponseMessage
    {
        public MusicResponse()
            : base("music")
        {

        }

        public MusicResponse(RequestMessage message)
            : base("music", message)
        {

        }


        public string Title { get; set; }
        public string Description { get; set; }
        public string MusicUrl { get; set; }
        public string HQMusicUrl { get; set; }
        public string ThumbMediaId { get; set; }

        protected override string InnerToxml()
        {
            string baseXml = base.InnerToxml();


            return baseXml + "<Music><Title>" + this.Title + "</Title><Description>" + this.Description + "</Description><MusicUrl>"
                 + this.MusicUrl + "</MusicUrl><HQMusicUrl>"
                 + this.HQMusicUrl + "</HQMusicUrl><ThumbMediaId>"
                 + this.ThumbMediaId + "</ThumbMediaId></Music>";

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    public class ImageResponse : ResponseMessage
    {
        public ImageResponse()
            : base("image")
        {

        }

        public string MediaId { get; set; }

        public ImageResponse(RequestMessage message)
            : base("image", message)
        {

        }

        protected override string InnerToxml()
        {
            string baseXml = base.InnerToxml();


            return baseXml + @"<Image><MediaId>" + MediaId + "</MediaId></Image>";
        }
    }
}

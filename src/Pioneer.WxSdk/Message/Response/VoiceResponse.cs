using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    public class VoiceResponse : ResponseMessage
    {
        public VoiceResponse()
            : base("voice")
        {

        }


        public string MediaId { get; set; }

        public VoiceResponse(RequestMessage message)
            : base("voice", message)
        {

        }

        protected override string InnerToxml()
        {
            string baseXml = base.InnerToxml();

            return baseXml + "<Voice><MediaId>" + this.MediaId + "</MediaId></Voice>";
        }
    }
}

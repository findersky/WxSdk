using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    public class TextResponse : ResponseMessage
    {
        public TextResponse()
            : base("text")
        {

        }

        public TextResponse(RequestMessage message)
            : base("text", message)
        {

        }


        public string Content
        {
            get;
            set;
        }

        protected override string InnerToxml()
        {
            string basexml = base.InnerToxml();
            return basexml + "<Content><![CDATA[" + this.Content + "]]></Content>";
        }
    }
}

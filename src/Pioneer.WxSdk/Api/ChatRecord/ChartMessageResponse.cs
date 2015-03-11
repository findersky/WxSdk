using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class ChartMessageResponse : Response
    {
        public ChartMessageResponse()
        {
            Recordlist = new List<ChatMessage>();
        }

        public List<ChatMessage> Recordlist
        {
            get;
            set;
        }
    }

    public class ChatMessage
    {
        public string Worker
        {
            get;
            set;
        }

        public string Opercode
        {
            get;
            set;
        }

        public string Time
        {
            get;
            set;
        }

        public string OpenId
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}

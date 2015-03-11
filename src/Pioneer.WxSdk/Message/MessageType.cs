using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{

    //    RequestMessageTypes.Add("text", typeof(TextRequest));
    //RequestMessageTypes.Add("image", typeof(ImageRequest));
    //RequestMessageTypes.Add("voice", typeof(VoiceRequest));
    //RequestMessageTypes.Add("video", typeof(VideoRequest));
    //RequestMessageTypes.Add("location", typeof(LocationRequest));
    //RequestMessageTypes.Add("link", typeof(LinkRequest));


    //RequestEventTypes.Add("subscribe", typeof(SubscribeEvent));
    //RequestEventTypes.Add("unsubscribe", typeof(EventRequest));
    //RequestEventTypes.Add("SCAN", typeof(SubscribeEvent));
    //RequestEventTypes.Add("LOCATION", typeof(LocationEvent));
    //RequestEventTypes.Add("CLICK", typeof(EventRequest));
    //RequestEventTypes.Add("VIEW", typeof(EventRequest));
    //RequestEventTypes.Add("MASSSENDJOBFINISH", typeof(MassSendFinishEvent));
    //RequestEventTypes.Add("Enter", typeof(EventRequest));

    public class MessageType
    {
        public const string Text = "text";

        public const string Image = "image";

        public const string Voice = "voice";

        public const string Video = "video";

        public const string Link = "link";

        public const string Location = "location";

        public const string Music = "music";

        public const string News = "news";

        public const string Event = "event";
    }

    public class EventType
    {
        public const string Subscribe = "subscribe";

        public const string Unsubscribe = "unsubscribe";

        public const string Scan = "scan";

        public const string click = "click";

        public const string View = "view";

        public const string MASSSENDJOBFINISH = "masssendjobfinish";

        public const string Enter = "enter";

        public const string Location = "location";
    }
}

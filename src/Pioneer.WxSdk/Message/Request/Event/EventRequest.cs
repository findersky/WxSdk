using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class EventRequest : RequestMessage
    {
        [XmlElement("EventKey")]
        public string EventKey
        {
            get;
            set;
        }

        [XmlElement("Event")]
        public string Event
        {
            get;
            set;
        }

    }
}

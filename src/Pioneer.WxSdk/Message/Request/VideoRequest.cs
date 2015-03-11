using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class VideoRequest : RequestMessage
    {
        [XmlElement("MediaId")]
        public string MediaId { get; set; }
        [XmlElement("ThumbMediaId")]
        public string ThumbMediaId { get; set; }

    }
}

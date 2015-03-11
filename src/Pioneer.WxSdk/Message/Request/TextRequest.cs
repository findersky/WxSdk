using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class TextRequest : RequestMessage
    {
        [XmlElement("Content")]
        public string Content { get; set; }
    }
}

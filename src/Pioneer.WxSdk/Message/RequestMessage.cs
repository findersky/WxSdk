using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class RequestMessage
    {
        [XmlElement("ToUserName")]
        public string ToUserName { get; set; }

        [XmlElement("FromUserName")]
        public string FromUserName { get; set; }

        [XmlElement("CreateTime")]
        public string CreateTimeString { get; set; }

        [XmlIgnore]
        public DateTime CreateTime { get; set; }

        [XmlElement("MsgType")]
        public string MsgType { get; set; }

        [XmlElement("MsgId")]
        public string MsgId { get; set; }

        [XmlElement("Encrypt")]
        public string Encrypt { get; set; }

        public override string ToString()
        {
            return this.SourceXml;
        }

        [XmlIgnore]
        public string SourceXml
        {
            get;
            set;
        }
    }
}

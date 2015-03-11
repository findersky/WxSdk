using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class ImageRequest : RequestMessage
    {
        [XmlElement("MediaId")]
        public string MediaId { get; set; }

        [XmlElement("PicUrl")]
        public string PicUrl { get; set; }


    }
}

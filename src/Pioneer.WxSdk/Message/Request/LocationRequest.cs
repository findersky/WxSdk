using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class LocationRequest : RequestMessage
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        [XmlElement("Location_X")]
        public decimal Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        [XmlElement("Location_Y")]
        public decimal Location_Y { get; set; }
        [XmlElement("Scale")]
        public int Scale { get; set; }
        [XmlElement("Label")]
        public string Label { get; set; }

    }
}

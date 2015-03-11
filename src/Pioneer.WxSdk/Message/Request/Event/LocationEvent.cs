using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class LocationEvent : EventRequest
    {
        [XmlElement("Latitude")]
        /// <summary>
        /// 地理位置维度，事件类型为LOCATION的时存在
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 地理位置经度，事件类型为LOCATION的时存在
        /// </summary>
        [XmlElement("Longitude")]
        public decimal Longitude { get; set; }
        /// <summary>
        /// 地理位置精度，事件类型为LOCATION的时存在
        /// </summary>
        [XmlElement("Precision")]
        public decimal Precision { get; set; }
    }
}

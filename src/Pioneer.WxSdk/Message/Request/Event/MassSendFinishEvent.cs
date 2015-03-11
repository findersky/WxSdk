using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class MassSendFinishEvent : EventRequest
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        [XmlElement("Status")]
        public string Status { get; set; }

        /// <summary>
        /// group_id下粉丝数；或者openid_list中的粉丝数
        /// </summary>
        [XmlElement("TotalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 过滤（过滤是指，有些用户在微信设置不接收该公众号的消息）后，准备发送的粉丝数，原则上，FilterCount = SentCount + ErrorCount
        /// </summary>
        [XmlElement("FilterCount")]
        public int FilterCount { get; set; }

        /// <summary>
        /// 发送成功的粉丝数
        /// </summary>
        [XmlElement("SendCount")]
        public int SendCount { get; set; }

        /// <summary>
        /// 发送失败的粉丝数
        /// </summary>
        [XmlElement("ErrorCount")]
        public int ErrorCount { get; set; }
    }
}

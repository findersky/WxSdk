using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    public class MchResponseEnums
    {
        public const string Success = "SUCCESS";

        public const string Fail = "FAIL";
    }

    [XmlRoot("xml")]
    public class PayResponse
    {
        [XmlIgnore]
        public bool Successed
        {
            get;
            set;
        }

        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        public string Xml
        {
            get;
            set;
        }

        [XmlElement("return_code")]
        public string ReturnCode
        {
            get;
            set;
        }

        [XmlElement("return_msg")]
        public string ReturnMessage
        {
            get;
            set;
        }

        [XmlElement("result_code")]
        public string ResultCode
        {
            get;
            set;
        }

        [XmlElement("err_code")]
        public string ErrorCode
        {
            get;
            set;
        }

        [XmlElement("err_code_des")]
        public string ErrorMessage
        {
            get;
            set;
        }

        [XmlElement("appid")]
        public virtual string AppId
        {
            get;
            set;
        }

        [XmlElement("mch_id")]
        public string Mch_id
        {
            get;
            set;
        }


    }
}

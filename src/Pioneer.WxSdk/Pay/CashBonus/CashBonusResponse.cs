using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    [XmlRoot("xml")]
    public class CashBonusResponse : PayResponse
    {

        public override string AppId
        {
            get
            {
                return WxAppid;
            }
        }


        [XmlElement("wxappid")]
        public string WxAppid
        {
            get;
            set;
        }



        [XmlElement("mch_billno")]
        public string OrderCode
        {
            get;
            set;
        }

        [Newtonsoft.Json.JsonIgnore]
        [XmlElement("total_amount")]
        public int Total_Amount
        {
            get;
            set;
        }

        [XmlIgnore]
        public decimal Amount
        {
            get
            {
                return (decimal)(Total_Amount / 100.0);
            }
        }

        [XmlElement("re_openid")]
        public string OpenId
        {
            get;
            set;
        }
    }
}

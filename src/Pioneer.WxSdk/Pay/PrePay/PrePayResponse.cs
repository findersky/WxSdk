using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    [XmlRoot("xml")]
    //[XmlInclude(typeof(MchResponse))]
    public class PrePayResponse : PayResponse
    {


        [XmlElement("nonce_str")]
        public string NonceString
        {
            get;
            set;
        }

        [XmlElement("sign")]
        public string Sign
        {
            get;
            set;
        }

        [XmlElement("trade_type")]
        public string TradeType
        {
            get;
            set;
        }

        [XmlElement("prepay_id")]
        public string PrePayId
        {
            get;
            set;
        }

    }
}

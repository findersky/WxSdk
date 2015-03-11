using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    [XmlRoot("xml")]
    public class RefundResponse : PayResponse
    {
        [XmlElement("transaction_id")]
        public string TransId
        {
            get;
            set;
        }

        [XmlElement("out_trade_no")]
        public string TradeCode
        {
            get;
            set;
        }

        [XmlElement("out_refund_no")]
        public string RefundTransCode
        {
            get;
            set;
        }

        [XmlElement("refund_id")]
        public string RefundId
        {
            get;
            set;
        }

        [XmlElement("refund_fee")]
        public int Refund_fee
        {
            get;
            set;
        }

        [XmlIgnore]
        public decimal RefundAmount
        {
            get
            {
                return (decimal)Refund_fee / 100;
            }
        }

    }
}

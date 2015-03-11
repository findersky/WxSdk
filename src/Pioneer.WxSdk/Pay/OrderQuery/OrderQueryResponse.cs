using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    [XmlRoot("xml")]
    public class OrderQueryResponse : PayResponse
    {
        [XmlElement("trade_state")]
        public string TradeState
        {
            get;
            set;
        }

        [XmlElement("openid")]
        public string OpenId
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

        [XmlElement("bank_type")]
        public string BankType
        {
            get;
            set;
        }

        [XmlElement("total_fee")]
        public int Total_fee
        {
            get;
            set;
        }

        [XmlIgnore]
        public decimal Amount
        {
            get
            {
                return (decimal)Total_fee / 100;
            }
        }

        [XmlElement("time_end")]
        public string Time_end
        {
            get;
            set;
        }

        [XmlIgnore]
        public DateTime TradeEndTime
        {
            get
            {
                DateTime dt;

                if (!DateTime.TryParseExact(this.Time_end, "yyyyMMddHHmmss", null, DateTimeStyles.None, out dt))
                {
                    dt = DateTime.MinValue;
                }

                return dt;
            }
        }

        [XmlElement("is_subscribe")]
        public string Is_Subscribe
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool IsSubScribe
        {
            get
            {
                if (Is_Subscribe == "Y")
                    return true;

                return false;
            }
        }

        [XmlElement("attach")]
        public string Attach
        {
            get;
            set;
        }
    }
}

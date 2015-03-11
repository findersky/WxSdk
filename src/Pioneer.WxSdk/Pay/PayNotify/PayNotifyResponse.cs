using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    [XmlRoot("xml")]
    public class PayNotifyResponse : PayResponse
    {
        [XmlElement("openid")]
        public string OpenId
        {
            get;
            set;
        }

        [XmlElement("is_subscribe")]
        public string Is_Subscribe
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool IsSubscribe
        {
            get
            {
                return Is_Subscribe == "Y";
            }
        }

        [XmlElement("bank_type")]
        public string BankType
        {
            get;
            set;
        }

        [XmlElement("fee_type")]
        public string Currency
        {
            get;
            set;
        }

        [XmlElement("total_fee")]
        public string TotalFee
        {
            set;
            get;
        }

        [XmlIgnore]
        public decimal Amount
        {
            get
            {
                decimal fee = decimal.Parse(this.TotalFee);

                return fee / 100;
            }
        }


        [XmlElement("transaction_id")]
        public string TransId
        {
            get;
            set;
        }

        [XmlElement("out_trade_no")]
        public string TransCode
        {
            get;
            set;
        }

        [XmlElement("attach")]
        public string Attach
        {
            get;
            set;
        }

        [XmlElement("time_end")]
        public string TimeEnd
        {
            get;
            set;
        }

        [XmlIgnore]
        public DateTime PayTime
        {
            get
            {

                DateTime dt;

                if (!DateTime.TryParseExact(this.TimeEnd, "yyyyMMddHHmmss", null, DateTimeStyles.None, out dt))
                    dt = DateTime.MinValue;

                return dt;
            }
        }

    }

}

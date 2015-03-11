using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    public class PayOrder
    {
        public PayOrder()
        {
            this.Encoding = Encoding.UTF8;
        }

        string _body;
        /// <summary>
        /// 订单描述 包括商品等信息
        /// </summary>
        public string Body
        {
            get
            {

                return _body;
            }
            set
            {
                _body = value;

                if (_body.Length > 40)
                    _body = _body.Substring(0, 40) + "...";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Attach
        {
            get;
            set;
        }

        public string Mch_id
        {
            get;
            set;
        }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string Code
        {
            get;
            set;
        }


        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }

        public string TotalFee
        {
            get
            {
                if (this.Amount <= 0)
                    throw new WxException("金额超出合法范围");

                decimal fee = Math.Round(Amount, 2);

                int feeint = (int)(fee * 100);

                return feeint.ToString();

            }
        }

        public Encoding Encoding
        {
            get;
            set;
        }

        public DateTime? StartTime
        {
            get;
            set;
        }

        public DateTime? EndTime
        {
            get;
            set;
        }

        public string Notify_Url
        {
            get;
            set;
        }

        public string ClientIp
        {
            get;
            set;
        }

        public string OpenId
        {
            get;
            set;
        }

        public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PaySignKey
        {
            get;
            set;
        }

    }
}

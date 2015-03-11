using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pioneer.WxSdk.Pay
{

    [Apipath("退款", "/pay/Refund")]
    public class RefundRequest : PayCertificateRequest<RefundResponse>
    {
        public string TransId
        {
            get;
            set;
        }

        public string OutTradeCode
        {
            get;
            set;
        }

        public string OutRefTradeCode
        {
            get;
            set;
        }

        public decimal Amount
        {
            get;
            set;
        }

        public decimal RefundAmount
        {
            get;
            set;
        }

        public string UserId
        {
            get;
            set;
        }

        protected override string Url
        {
            get { return "https://api.mch.weixin.qq.com/secapi/pay/refund"; }
        }

        protected override RefundResponse DoResponse()
        {
            IDictionary<string, string> dic = this.CreatePostData();

            dic.Add("out_trade_no", this.OutTradeCode);
            dic.Add("out_refund_no", this.OutRefTradeCode);
            dic.Add("total_fee", PayHelp.ToWxPayAmount(this.Amount));
            dic.Add("refund_fee", PayHelp.ToWxPayAmount(this.RefundAmount));
            dic.Add("op_user_id", this.UserId);
            dic.Add("transaction_id", this.TransId);
            Sign(dic);

            return this.HttpPost(dic);

        }


        protected override void DoValidate()
        {
            base.DoValidate();


            if (string.IsNullOrEmpty(this.OutRefTradeCode))
            {
                throw new ArgumentNullException("out refund trade no");
            }

            if (this.RefundAmount > this.Amount)
            {
                throw new WxException("退款金额超过整单金额");
            }

            if (string.IsNullOrEmpty(this.OutTradeCode))
            {
                throw new ArgumentNullException("out trade no");
            }

            if (string.IsNullOrEmpty(this.UserId))
            {
                throw new ArgumentNullException("userid");
            }

        }

    }
}

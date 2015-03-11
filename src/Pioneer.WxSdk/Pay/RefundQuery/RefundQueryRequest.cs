using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    [Apipath("退款查询", "/pay/RefundQuery")]
    public class RefundQueryRequest : PayRequest<RefundQueryResponse>
    {
        public string TransId
        {
            get;
            set;
        }

        public string OutTransCode
        {
            get;
            set;
        }

        public string RefundId
        {
            get;
            set;
        }

        public string OutRefundCode
        {
            get;
            set;
        }



        protected override string Url
        {
            get { return "https://api.mch.weixin.qq.com/pay/refundquery"; }
        }

        protected override RefundQueryResponse DoResponse()
        {
            IDictionary<string, string> dic = this.CreatePostData();

            dic.Add("refund_id", this.RefundId);
            dic.Add("out_trade_no", this.OutRefundCode);
            dic.Add("out_refund_no", this.OutTransCode);
            dic.Add("transaction_id", this.TransId);

            Sign(dic);

            return this.HttpPost(dic);


        }

        protected override void DoValidate()
        {
            base.DoValidate();

            if (string.IsNullOrEmpty(this.RefundId) && string.IsNullOrEmpty(this.TransId)
    && string.IsNullOrEmpty(this.OutTransCode) && string.IsNullOrEmpty(this.OutRefundCode))
            {
                throw new ArgumentNullException();
            }
        }
    }
}

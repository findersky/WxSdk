using Pioneer.WxSdk.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    [Apipath("查询订单", "/pay/OrderQuery")]
    public partial class OrderQueryRequest : PayRequest<OrderQueryResponse>
    {
        /// <summary>
        /// 微信交易Id
        /// </summary>
        public string TransId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户订单号
        /// </summary>
        public string OutTradeCode
        {
            get;
            set;
        }


        protected override string Url
        {
            get { return "https://api.mch.weixin.qq.com/pay/orderquery"; }
        }


        protected override OrderQueryResponse DoResponse()
        {
            IDictionary<string, string> postData = this.CreatePostData();
            postData.Add("transaction_id", this.TransId);
            postData.Add("out_trade_no", this.OutTradeCode);
            Sign(postData);
            return this.HttpPost(postData);
        }

        protected override void DoValidate()
        {
            if (string.IsNullOrEmpty(this.OutTradeCode))
            {
                throw new ArgumentNullException("Out Trade Code");
            }
        }
    }
}

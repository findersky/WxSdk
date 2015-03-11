using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    [Apipath("预支付订单", "/pay/PrePay")]
    public class PrePayRequest : PayRequest<PrePayResponse>
    {
        public override string AppId
        {
            get
            {
                if (this.order == null)
                    return string.Empty;

                return this.order.AppId;
            }
            set
            {
                throw new Exception("此参数必须通过Order设置");
            }
        }

        public override string MchId
        {
            get
            {
                if (this.order == null)
                    return string.Empty;

                return this.order.Mch_id;
            }
            set
            {
                throw new Exception("此参数必须通过Order设置");
            }
        }

        public override string PaySignKey
        {
            get
            {
                if (this.order == null)
                    return string.Empty;

                return this.order.PaySignKey;
            }
            set
            {
                throw new Exception("此参数必须通过Order设置");
            }
        }


        public PayOrder order
        {
            get;
            set;
        }

        protected override string Url
        {
            get
            {
                return "https://api.mch.weixin.qq.com/pay/unifiedorder";
            }
        }

        public string TradeType
        {
            get
            {
                return "JSAPI";
            }
        }


        protected override void DoValidate()
        {
            if (order == null)
            {
                throw new ArgumentNullException("订单信息order不能为null");
            }
            else if (string.IsNullOrWhiteSpace(order.Body))
            {
                throw new ArgumentNullException("商品描述不能为空");
            }
            else if (string.IsNullOrWhiteSpace(order.Code))
            {
                throw new ArgumentNullException("订单号不能为空");
            }
            else if (order.Code.Length >= 32)
            {
                throw new ArgumentException("订单号必须在32字节以下");
            }
            else if (string.IsNullOrWhiteSpace(order.ClientIp))
            {
                throw new ArgumentNullException("用户客户端IP不能为空");
            }
            else if (order.ClientIp.IndexOf(".") < 0)
            {
                throw new ArgumentException("用户客户端IP不合法");
            }
            else if (order.Amount <= 0)
            {
                throw new ArgumentException("订单总金额超出合法范围");
            }
            else if (string.IsNullOrEmpty(order.AppId))
            {
                throw new ArgumentNullException("appid");
            }
            else if (string.IsNullOrEmpty(order.Mch_id))
            {
                throw new ArgumentNullException("mch_id");
            }
            else if (string.IsNullOrEmpty(order.PaySignKey))
            {
                throw new ArgumentNullException("pay key");
            }
            else if (this.TradeType == "JSAPI" && string.IsNullOrEmpty(this.order.OpenId))
            {
                throw new ArgumentNullException("openid");
            }
        }


        protected override PrePayResponse DoResponse()
        {
            IDictionary<string, string> sk = this.CreatePostData();

            sk.Add("body", this.order.Body);
            sk.Add("openid", this.order.OpenId);
            sk.Add("out_trade_no", this.order.Code);
            sk.Add("total_fee", this.order.TotalFee);
            sk.Add("spbill_create_ip", this.order.ClientIp);
            sk.Add("notify_url", this.order.Notify_Url);
            sk.Add("trade_type", this.TradeType);
            sk.Add("attach", this.order.Attach);
            this.Sign(sk);
            return this.HttpPost(sk);
        }
    }
}

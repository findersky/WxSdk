using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    [Apipath("关闭订单", "/pay/orderclose")]
    public class OrderCloseRequest : PayRequest<PayResponse>
    {

        protected override string Url
        {
            get { return "https://api.mch.weixin.qq.com/pay/closeorder"; }
        }

        protected override PayResponse DoResponse()
        {
            throw new NotImplementedException();
        }
    }
}

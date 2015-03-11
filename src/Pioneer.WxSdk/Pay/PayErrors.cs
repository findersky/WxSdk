using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    public static class PayErrors
    {
        static PayErrors()
        {
            es = new Dictionary<string, string>();
            es.Add("SYSTEMERROR", "接口后台错误");
            es.Add("INVALID_TRANSACTIONID", "无效 transaction");
            es.Add("PARAM_ERROR", "提交参数错误");
            es.Add("ORDERPAID", "订单已支付");
            es.Add("OUT_TRADE_NO_USED", "商户订单号重复");
            es.Add("NOAUTH", "商户无权限");
            es.Add("NOTENOUGH", "余额不足");
            es.Add("NOTSUPORTCARD", "不支持卡类型");
            es.Add("ORDERCLOSED", "订单已关闭");
            es.Add("BANKERROR", "银行系统异常");
            es.Add("REFUND_FEE_INVALID", "退款金额大于支付金额");
            es.Add("ORDERNOTEXIST", "ORDERNOTEXIST");

            es.Add("OPENID_ERROR", "openid错误");
            es.Add("SYSTEMERROR", "系统错误");
            es.Add("TIME _LIMITED", "企业红包的収送时间受限");
            es.Add("DAY_ OVER_LIMITED", "企业红包的日限额受限");
            es.Add("SECOND_OVER_LIMITED", "企业红包的按分钟収放受限");

        }

        static Dictionary<string, string> es;

    }
}

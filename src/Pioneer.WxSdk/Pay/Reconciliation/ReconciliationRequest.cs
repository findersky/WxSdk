using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    /// <summary>
    /// 对账单接口太恶心 不能用
    /// </summary>
    public class ReconciliationRequest : PayRequest<ReconciliationResponse>
    {
        protected override string Url
        {
            get { return "https://api.mch.weixin.qq.com/pay/downloadbill"; }
        }

        public DateTimeOffset BillDate
        {
            get;
            set;
        }

        protected override ReconciliationResponse DoResponse()
        {
            throw new NotImplementedException();
        }

        protected override void DoValidate()
        {
            if (this.BillDate == default(DateTimeOffset))
            {
                throw new ArgumentNullException("bill date");
            }
        }

        protected override ReconciliationResponse Deserialize(string xml)
        {
            ReconciliationResponse r = new ReconciliationResponse();

            r.Xml = xml;

            return r;
        }
    }
}

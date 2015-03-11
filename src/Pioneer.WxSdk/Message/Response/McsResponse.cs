using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    /// <summary>
    /// 多客服回复
    /// </summary>
    public class McsResponse : ResponseMessage
    {
        public McsResponse()
            : base("transfer_customer_service")
        {

        }

        public McsResponse(RequestMessage message)
            : base("transfer_customer_service", message)
        {

        }
    }
}

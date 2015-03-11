using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    public interface IMessageListener : IDisposable
    {
        ResponseMessage ProcessRequest(RequestMessage request);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Pioneer.WxSdk
{
    class HttpResponseMessage
    {
        public HttpResponseMessage()
        {
            string s = System.Net.HttpRequestHeader.UserAgent.ToString();
            Console.WriteLine(s);

        }
    }
}

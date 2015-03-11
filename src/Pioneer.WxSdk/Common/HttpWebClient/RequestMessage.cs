using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    public class HttpRequestMessage
    {
        public HttpRequestMessage()
        {
            this.Headers = new NameValueCollection();

        }
        public string Uri
        {
            get;
            set;
        }

        public NameValueCollection Headers
        {
            get;
            set;
        }

        public string HttpMethod
        {
            get;
            set;
        }

        public byte[] Body
        {
            get;
            set;
        }

        public Encoding Encoding
        {
            get;
            set;
        }
    }
}

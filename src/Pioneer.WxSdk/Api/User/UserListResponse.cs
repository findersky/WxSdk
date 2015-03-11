using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class UserListResponse : Response
    {
        public int Total { get; set; }
        public int Count { get; set; }
        public OpenIdCollection Data { get; set; }
        public string Next_openid { get; set; }


        public class OpenIdCollection
        {
            public OpenIdCollection()
            {
                OpenId = new List<string>();
            }
            public List<string> OpenId
            {
                get;
                set;
            }
        }
    }
}

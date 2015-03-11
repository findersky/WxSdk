using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class JsTicketResponse : Response
    {
        public string Ticket
        {
            get;
            set;
        }

        public int Expires_in
        {
            get;
            set;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class QrCodeResponse : Response
    {
        public string Ticket { get; set; }
        public int Expire_seconds { get; set; }

        public string QrCodeUrl
        {
            get;
            set;
        }

        public byte[] QrCodeData
        {
            get;
            set;
        }
    }
}

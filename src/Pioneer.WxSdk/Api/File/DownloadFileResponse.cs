using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class DownloadFileResponse : Response
    {
        public byte[] Data
        {
            get;
            set;
        }
    }
}

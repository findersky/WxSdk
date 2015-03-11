using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class UpLoadNewsResponse : Response
    {
        public string Type
        {
            get;
            set;
        }

        public string Media_id
        {
            get;
            set;
        }

        public string Created_at
        {
            get;
            set;
        }
    }
}

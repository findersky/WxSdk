using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class UploadVideoResponse : Response
    {
        public string Type
        {
            get;
            set;
        }

        public string Media_Id
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

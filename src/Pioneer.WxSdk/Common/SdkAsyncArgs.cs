using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    public class SdkAsyncArgs
    {
        public Exception Error
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public object Data
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    public class WxException : Exception
    {
        public string ErrorCode
        {
            get;
            set;
        }

        public WxException(string message)
            : base(message)
        {

        }

        public WxException(string code, string message)
            : base(message)
        {
            this.ErrorCode = code;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public abstract class DataRequest<T> : Request<T>
        where T : Response
    {
        public DateTime Begin
        {
            get;
            set;
        }

        public DateTime End
        {
            get;
            set;
        }
    }
}

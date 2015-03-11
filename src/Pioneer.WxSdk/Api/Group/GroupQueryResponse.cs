using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class GroupQueryResponse : Response
    {
        public GroupQueryResponse()
        {
            this.Groups = new List<GroupResponse>();
        }

        public List<GroupResponse> Groups
        {
            get;
            set;
        }
    }
}

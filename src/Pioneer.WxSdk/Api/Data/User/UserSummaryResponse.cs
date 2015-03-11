using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class UserSummaryResponse : Response
    {

        public UserSummaryResponse()
        {
            this.List = new List<UserSummaryResponseItem>();
        }
        public List<UserSummaryResponseItem> List
        {
            get;
            set;
        }
    }

    public class UserSummaryResponseItem
    {
        public DateTime Ref_Date
        {
            get;
            set;
        }

        public int User_source
        {
            get;
            set;
        }

        [Newtonsoft.Json.JsonIgnore]
        public string UserSource
        {
            get
            {
                if (User_source == 0)
                    return "其他";

                if (User_source == 30)
                    return "二维码扫描";

                if (User_source == 17)
                    return "名片分享";

                if (User_source == 35)
                    return "搜号码（即微信添加朋友页的搜索）";

                if (User_source == 39)
                    return "查询微信公众帐号";

                if (User_source == 43)
                    return "图文页右上角菜单 ";
                return "其他";

            }
        }

        public int New_user
        {
            get;
            set;
        }

        public int Cancel_user
        {
            get;
            set;
        }

        public int Cumulate_user
        {
            get;
            set;
        }
    }

}

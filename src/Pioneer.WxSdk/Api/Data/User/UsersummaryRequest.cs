using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class UsersummaryRequest : DataRequest<UserSummaryResponse>
    {
        public override string Url
        {
            get
            {
                return string.Format(
                    "https://api.weixin.qq.com/datacube/getusersummary?access_token={0}", this.AccessToken);
            }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if ((this.End - this.Begin).TotalDays > 7)
            {
                return Tuple.Create(false, "日期范围最大为7天");
            }

            return base.DoValidate();
        }

        protected override UserSummaryResponse DoResponse()
        {
            var obj = new { begin_date = Begin.ToString("yyyy-MM-dd"), end_date = End.ToString("yyyy-MM-dd") };

            string json = this.Serialize(obj);

            UserSummaryResponse ur = this.HttpPost(json);

            string totalUrl = string.Format("https://api.weixin.qq.com/datacube/getusercumulate?access_token={0}", this.AccessToken);

            UserSummaryResponse totalur = this.HttpPost(json, totalUrl);


            foreach (var x in ur.List)
            {
                var c = totalur.List.FirstOrDefault(o => o.Ref_Date == x.Ref_Date);

                if (c == null)
                    continue;

                x.Cumulate_user = c.Cumulate_user;
            }

            return ur;

        }
    }
}

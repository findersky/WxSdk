using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("获取关注者列表", "/api/UserList")]
    public class UserListRequest : Request<UserListResponse>
    {

        public override string Url
        {
            get
            {
                return string.Format(@"https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}", this.AccessToken);
            }
        }

        string PagedUrl(string nextId)
        {
            return string.Format(@"https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", this.AccessToken, nextId);
        }


        protected override UserListResponse DoResponse()
        {
            using (HttpWebClient client = new HttpWebClient())
            {
                string json = client.DownloadString(this.Url);

                logger.Info(json);

                UserListResponse ulr = this.Deserialize(json);

                if (ulr.Count < ulr.Total)
                {
                    do
                    {
                        string pageJson = client.DownloadString(this.PagedUrl(ulr.Next_openid));

                        UserListResponse newUlr = this.Deserialize(pageJson);

                        ulr.Count = ulr.Count + newUlr.Count;

                        ulr.Data.OpenId.AddRange(newUlr.Data.OpenId);
                        ulr.Next_openid = newUlr.Next_openid;
                    }
                    while (ulr.Count >= ulr.Total);
                }

                return ulr;
            }
        }
    }
}

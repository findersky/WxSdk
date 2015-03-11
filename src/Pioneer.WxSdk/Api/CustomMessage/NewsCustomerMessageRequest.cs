using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("发送客服消息,图文", "/api/NewsCustomerMessage")]
    public class NewsCustomerMessageRequest : CustomMessageRequest
    {
        public NewsCustomerMessageRequest()
        {
            this.Articles = new List<Article>();
        }
        public List<Article> Articles
        {
            get;
            set;
        }


        protected override object GetMessage()
        {
            return new
            {
                touser = this.OpenId,
                msgtype = "news",
                news = new
                {
                    articles = this.Articles.Select(z => new
                    {
                        title = z.Title,
                        description = z.Description,
                        url = z.Url,
                        picurl = z.PicUrl
                    })
                }
            };
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (this.Articles.Count == 0)
                return Tuple.Create(false, "Articles.Count=0");

            return base.DoValidate();
        }
    }
}

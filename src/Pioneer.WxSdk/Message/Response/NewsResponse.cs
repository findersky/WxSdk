using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    public class NewsResponse : ResponseMessage
    {
        public NewsResponse()
            : base("news")
        {
            this.Articles = new List<Article>();
        }

        public NewsResponse(RequestMessage message)
            : base("news", message)
        {
            this.Articles = new List<Article>();
        }

        public List<Article> Articles { get; set; }



        protected override string InnerToxml()
        {
            string baseXml = base.InnerToxml();

            string axml = " <ArticleCount>" + Articles.Count + "</ArticleCount><Articles>";


            foreach (var x in Articles)
            {
                axml += GetArticleXml(x);
            }

            axml += "</Articles>";

            return baseXml + axml;
        }

        string GetArticleXml(Article ar)
        {
            string template = @"<item>
<Title><![CDATA[{0}]]></Title>
<Description><![CDATA[{1}]]></Description>
<PicUrl><![CDATA[{2}]]></PicUrl>
<Url><![CDATA[{3}]]></Url>
</item>";

            return string.Format(template, ar.Title, ar.Description, ar.PicUrl, ar.Url);

        }

    }
}

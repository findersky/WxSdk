using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("上传图文消息", "/api/UpLoadNews")]
    public class UpLoadNewsRequest : Request<UpLoadNewsResponse>
    {

        public UpLoadNewsRequest()
        {
            this.News = new List<UploadNews>();
        }

        public List<UploadNews> News
        {
            get;
            set;
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (this.News.Count == 0)
            {
                return Tuple.Create(false, "News.count==0");
            }

            foreach (var n in News)
            {
                if (string.IsNullOrEmpty(n.Title))
                {
                    return Tuple.Create(false, "News.Title is null");
                }

                if (string.IsNullOrEmpty(n.Content))
                {
                    return Tuple.Create(false, "News.Content is null");
                }

                if (string.IsNullOrEmpty(n.ThumbMediaId))
                {
                    return Tuple.Create(false, "News.ThumbMediaId is null");
                }
            }

            return base.DoValidate();
        }

        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}", this.AccessToken); }
        }

        protected override UpLoadNewsResponse DoResponse()
        {
            var jsonData = new
            {
                articles = News.Select(x => new
                {
                    thumb_media_id = x.ThumbMediaId,
                    author = x.Author,
                    title = x.Title,
                    content_source_url = x.SourceUrl,
                    content = x.Content,
                    digest = x.Digest,
                    show_cover_pic = x.ShowCoverImage ? "1" : "0"
                })
            };

            return this.HttpPost(this.Serialize(jsonData));
        }
    }

    public class UploadNews
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ThumbMediaId
        {
            get;
            set;
        }

        [Required]
        public string Title
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }
        [Required]
        public string Content
        {
            get;
            set;
        }

        public string SourceUrl
        {
            get;
            set;
        }

        public string Digest
        {
            get;
            set;
        }

        /// <summary>
        /// 正文显示封面图片
        /// </summary>
        public bool ShowCoverImage
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    /// <summary>
    /// 单个多图文信息的内容
    /// </summary>
    [Serializable]
    public class WxMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 正文内容  这是一段Html
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 缩略图文描述
        /// </summary>
        public string Digest { get; set; }
        /// <summary>
        /// 作者  这个参数不用设置
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 封面图片Id  图片需要先上传到微信服务器  上传完成后返回这个id
        /// </summary>
        public string ConverMediaId { get; set; }
        /// <summary>
        /// 是否显示封面图片
        /// </summary>
        public bool ShowCoverPic { get; set; }
        /// <summary>
        /// 原文链接
        /// </summary>
        public string SourceUrl { get; set; }

        public WxMessage(WxMessage message)
        {
            Title = message.Title;
            Content = message.Content;
            Digest = message.Digest;
            Author = message.Author;
            ConverMediaId = message.ConverMediaId;
            ShowCoverPic = message.ShowCoverPic;
            SourceUrl = message.SourceUrl;
            this.CoverImage = message.CoverImage;
        }

        /// <summary>
        /// 封面图片路径
        /// </summary>
        public string CoverImage
        {
            get;
            set;
        }

        
        public object Tag
        {
            get;
            set;
        }

        public WxMessage()
        {
            // TODO: Complete member initialization
        }
    }
}

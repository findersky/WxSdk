using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    [Serializable]
    [XmlRoot("xml")]
    public class Feedback
    {
        public Feedback()
        {

        }



        public bool IsNew
        {
            get
            {
                return this.MsgType == "request";
            }
        }

        public bool IsConform
        {
            get
            {
                return this.MsgType == "confirm";
            }
        }

        public bool IsReject
        {
            get 
            {
                return this.MsgType == "reject";
            }
        }


        public string OpenId
        {
            get;
            set;
        }

        public string AppId
        {
            get;
            set;
        }

        public string TimeStamp
        {
            get;
            set;
        }

        public string MsgType
        {
            get;
            set;
        }

        public string FeedBackId
        {
            get;
            set;
        }

        public string TransId
        {
            get;
            set;
        }

        public string Reason
        {
            get;
            set;
        }

        public string Soulition
        {
            get;
            set;
        }

        public string ExtInfo
        {
            get;
            set;
        }

        public string AppSignature
        {
            get;
            set;
        }

        public string SignMothed
        {
            get;
            set;
        }

        [XmlArrayItem("item")]

        public List<FeedBackPicUrl> PicInfo
        {
            get;
            set;
        }

        [XmlIgnore]
        public IEnumerable<string> Pics
        {
            get
            {
                if (PicInfo == null || PicInfo.Count == 0)
                    return null;

                return PicInfo.Select(x => x.PicUrl).Where(o => !string.IsNullOrEmpty(o));
            }
        }
    }

    public class FeedBackPicUrl
    {
        public FeedBackPicUrl()
        {

        }

        public FeedBackPicUrl(string url)
        {
            this.PicUrl = url;
        }

        public string PicUrl
        {
            get;
            set;
        }
    }
}

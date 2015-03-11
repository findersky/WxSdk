using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("下载多媒体文件", "/api/DownloadFile")]
    public class DownloadFileRequest : Request<DownloadFileResponse>
    {
        public string Media_id
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", this.AccessToken, this.Media_id); }
        }


        protected override DownloadFileResponse DoResponse()
        {
            using (HttpWebClient client = new HttpWebClient())
            {
                DownloadFileResponse dr = new DownloadFileResponse();

                byte[] data = client.DownloadData(this.Url);

                if (data.Length < 50)
                {
                    string json = client.Encoding.GetString(data);

                    dr = this.Deserialize(json);
                }


                dr.Data = data;

                return dr;
            }
        }
    }
}

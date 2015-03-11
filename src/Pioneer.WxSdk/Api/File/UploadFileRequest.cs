using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class FileTypes
    {
        public const string Image = "image";
        public const string Voice = "voice";
        public const string Video = "video";
        public const string Thumb = "thumb";

    }

    [Apipath("上传多媒体文件", "/api/UploadFile")]
    public class UploadFileRequest : Request<UploadFileResponse>
    {
        public string FilePath
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", this.AccessToken, this.Type); }
        }

        protected override Tuple<bool, string> DoValidate()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                return Tuple.Create(false, "filePath is null");
            }


            if (!FilePath.StartsWith("http"))
            {
                if (!File.Exists(FilePath))
                {
                    return Tuple.Create(false, "filepath no exists");
                }
            }

            if (this.Type != FileTypes.Image && this.Type != FileTypes.Voice && this.Type != FileTypes.Video && this.Type != FileTypes.Thumb)
            {
                return Tuple.Create(false, "not support file type" + this.Type);
            }

            return base.DoValidate();
        }

        protected override UploadFileResponse DoResponse()
        {
            using (HttpWebClient client = new HttpWebClient())
            {
                string localpath = "";
                bool deletelocalfile = false;
                if (this.FilePath.StartsWith("http"))
                {
                    string temp = System.Environment.GetEnvironmentVariable("TEMP");
                    string fName = Path.GetFileName(this.FilePath);

                    localpath = Path.Combine(temp, fName);

                    Console.WriteLine(localpath);

                    deletelocalfile = true;

                    client.DownloadFile(this.FilePath, localpath);
                }
                else
                {
                    localpath = this.FilePath;
                }


                byte[] data = client.UploadFile(this.Url, localpath);

                string json = client.Encoding.GetString(data);

                if (deletelocalfile)
                {
                    File.Delete(localpath);
                }

                return this.Deserialize(json);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("生成带参数二维码", "/api/QrCode")]
    public class QrCodeRequest : Request<QrCodeResponse>
    {
        public QrCodeRequest()
            : base()
        {
            this.ExpireSeconds = null;
        }

        public int SenseId
        {
            get;
            set;
        }

        public int? ExpireSeconds
        {
            get;
            set;
        }


        public override string Url
        {
            get { return string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", this.AccessToken); }
        }

        protected override Tuple<bool, string> DoValidate()
        {

            if (SenseId > 100000 || SenseId <= 0)
            {
                return Tuple.Create(false, "sceneId 合法值为 1--100000,当前值" + SenseId.ToString());
            }

            return base.DoValidate();
        }

        protected override QrCodeResponse DoResponse()
        {
            string json = this.CreateJson();

            using (HttpWebClient client = new HttpWebClient())
            {

                string result = client.UploadString(Url, json);

                this.logger.Info(result);

                QrCodeResponse qr = this.Deserialize(result);

                qr.QrCodeUrl = string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", qr.Ticket);

                qr.QrCodeData = client.DownloadData(qr.QrCodeUrl);

                return qr;
            }
        }

        string CreateJson()
        {
            object data = null;
            if (this.ExpireSeconds == null)
            {
                data = new
                {
                    action_name = "QR_LIMIT_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = this.SenseId
                        }
                    }
                };
            }
            else
            {

                data = new
                {
                    expire_seconds = this.ExpireSeconds,
                    action_name = "QR_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = this.SenseId
                        }
                    }
                };
            }

            return this.Serialize(data);

        }

    }
}

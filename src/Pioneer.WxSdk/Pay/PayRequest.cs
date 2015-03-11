using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    public abstract class PayRequest<T> : IRequest
        where T : PayResponse, new()
    {

        public virtual string AppId
        {
            get;
            set;
        }

        public virtual string MchId
        {
            get;
            set;
        }

        public virtual string PaySignKey
        {
            get;
            set;
        }




        protected abstract string Url
        {
            get;
        }

        protected virtual void Vaildate()
        {
            if (string.IsNullOrEmpty(this.AppId))
            {
                throw new ArgumentNullException("AppId");
            }

            if (string.IsNullOrEmpty(this.MchId))
            {
                throw new ArgumentNullException("MchId");
            }

            this.DoValidate();
        }

        protected virtual void DoValidate()
        {

        }

        protected T HttpPost(IDictionary<string, string> postdata)
        {
            string xml = XmlService.WriteXml(postdata);

            return HttpPost(xml);
        }

        protected T HttpPost(string xml)
        {
            string retXml = this.OnPosting(xml);

            T t = Deserialize(retXml);

            return OnPost(t);
        }

        protected virtual string OnPosting(string xml)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string retXml = wc.UploadString(Url, "POST", xml);
            return retXml;
        }

        protected virtual T Deserialize(string xml)
        {
            T t = XmlService.FromXml<T>(xml);
            t.Xml = xml;

            return t;
        }

        protected virtual T OnPost(T t)
        {

            if (t.ReturnCode != MchResponseEnums.Success)
            {
                throw new WxException(t.ErrorCode, t.ErrorMessage);
            }

            if (t.ResultCode != MchResponseEnums.Success)
            {
                throw new WxException(t.ErrorCode, t.ErrorMessage);
            }

            return t;
        }


        public virtual T GetResponse()
        {
            if (SdkConfig.Instance.Deploy == Deploy.Server)
            {
                return LocalResponse();
            }
            else
            {
                return RemoteResponse();
            }

        }

        T RemoteResponse()
        {

            string remotePath = this.getRemotePath();

            if (string.IsNullOrEmpty(remotePath))
                return LocalResponse();

            string json = JsonService.ToJson(this);

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;



            string retjson = client.UploadString(SdkConfig.Instance.RemoteServer.TrimEnd('/') + remotePath, "POST", json);
            T rsp = JsonService.FromJson<T>(retjson);

            if (rsp.Successed)
            {
                return rsp;
            }
            else
            {
                throw new WxException(rsp.ErrorCode, rsp.ErrorMessage);
            }
        }

        string getRemotePath()
        {

            var ats = this.GetType().GetCustomAttributes(typeof(ApipathAttribute), false);

            if (ats.Length == 0)
                return null;

            return (ats[0] as ApipathAttribute).Path;
        }


        T LocalResponse()
        {
            try
            {

                this.Vaildate();

                return DoResponse();
            }
            catch (WxException we)
            {
                ILog log = LogFactory.GetLogger(this.GetType());

                log.Error(we);

                log.Error(we.ErrorCode);
                log.Error(we.Message);
                throw;
            }
            catch (Exception e)
            {
                ILog log = LogFactory.GetLogger(this.GetType());

                log.Error(e);

                throw;
            }
        }


        protected abstract T DoResponse();

        protected IDictionary<string, string> CreatePostData()
        {
            StringKeyValueCollection sk = new StringKeyValueCollection();

            sk.Add("appid", this.AppId);
            sk.Add("mch_id", this.MchId);
            sk.Add("nonce_str", PayHelp.NonceString);

            return sk;
        }


        protected void Sign(IDictionary<string, string> Postdata)
        {
            string sign = PayHelp.Sign(Postdata, this.PaySignKey);
            Postdata.Add("sign", sign);
        }

        object IRequest.GetResponse()
        {
            return this.GetResponse();
        }
    }
}

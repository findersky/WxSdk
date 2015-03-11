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
    /// <summary>
    /// Https请求  需要证书
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PayCertificateRequest<T> : PayRequest<T>
          where T : PayResponse, new()
    {
        /// <summary>
        /// 证书序列号
        /// </summary>
        public string CertificateNo
        {
            get;
            set;
        }



        protected override string OnPosting(string xml)
        {
            if(string.IsNullOrEmpty(CertificateNo))
            {
                throw new ArgumentNullException("CertificateNo");
            }

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(this.Url);
            webrequest.Credentials = CredentialCache.DefaultNetworkCredentials;
            webrequest.ClientCertificates.Add(QueryCret(this.CertificateNo));
            webrequest.Method = "POST";

            var s = webrequest.GetRequestStream();
            using (StreamWriter sw = new StreamWriter(s))
            {
                sw.Write(xml);
                sw.Flush();
                s.Flush();
            }

            HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
            Stream stream = webreponse.GetResponseStream();
            string resp = string.Empty;
            using (StreamReader reader = new StreamReader(stream))
            {
                resp = reader.ReadToEnd();
            }

            return resp;
        }

        bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        X509Certificate2 QueryCret(string sn)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);

            store.Open(OpenFlags.MaxAllowed);

            foreach (var x in store.Certificates)
            {
                if (x.SerialNumber.ToLower() == sn)
                {
                    return x;
                }
            }

            throw new WxException("找不到证书" + sn);
        }

    }
}

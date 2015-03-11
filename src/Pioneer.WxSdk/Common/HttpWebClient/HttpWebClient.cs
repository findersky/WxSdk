

using System.IO;
using System.Net;
namespace Pioneer.WxSdk
{
    public class HttpWebClient : WebClient
    {
        public HttpWebClient(string url)
            : base()
        {
            this.Url = url;
            this.Encoding = System.Text.Encoding.UTF8;
        }

        public HttpWebClient()
            : base()
        {
            this.Encoding = System.Text.Encoding.UTF8;
        }

        public string HttpMethod
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public CookieContainer CookieContainer
        {
            get;
            set;
        }

        protected override WebRequest GetWebRequest(System.Uri address)
        {
            HttpWebRequest wr = (HttpWebRequest)base.GetWebRequest(address);

            if (this.CookieContainer == null)
                this.CookieContainer = new System.Net.CookieContainer();
            wr.CookieContainer = CookieContainer;
            return wr;

        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            return (HttpWebResponse)request.GetResponse();
        }

        public byte[] UploadData(HttpRequestMessage message)
        {
            this.Headers.Add(message.Headers);

            if (string.IsNullOrEmpty(message.HttpMethod))
            {
                message.HttpMethod = this.HttpMethod;
            }

            return this.UploadData(message.Uri, message.HttpMethod, message.Body);
        }

        public void UploadDataAsync(HttpRequestMessage message)
        {
            this.Headers.Add(message.Headers);

            if (string.IsNullOrEmpty(message.HttpMethod))
            {
                message.HttpMethod = this.HttpMethod;
            }

            this.UploadDataAsync(new System.Uri(message.Uri), message.HttpMethod, message.Body);
        }

        public string DownloadString(HttpRequestMessage message)
        {
            this.Headers.Add(message.Headers);

            if (string.IsNullOrEmpty(message.HttpMethod))
            {
                message.HttpMethod = this.HttpMethod;
            }
            return this.DownloadString(message.Uri);
        }

        public void DownloadStringAsync(HttpRequestMessage message)
        {
            this.Headers.Add(message.Headers);

            if (string.IsNullOrEmpty(message.HttpMethod))
            {
                message.HttpMethod = this.HttpMethod;
            }
            this.DownloadStringAsync(new System.Uri(this.Url));
        }

        public static string UrlEncode(string s)
        {
            return System.Web.HttpUtility.UrlEncode(s);
        }

    }
}
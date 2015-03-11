using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.WxSdk.Api
{
    public abstract class Request<T> : IRequest
        where T : Response
    {
        protected ILog logger;

        public Request()
        {
            this.logger = LogFactory.GetLogger(this.GetType());
        }

        public PublicAccount PublicAccountInfo
        {
            get;
            set;
        }

        protected string AccessToken
        {
            get
            {
                if (this.PublicAccountInfo != null)
                    return this.PublicAccountInfo.AccessToken;

                throw new Exception("Request.TokenInfo is Null");
            }
        }

        public abstract string Url
        {
            get;
        }

        public virtual Tuple<bool, string> Validate()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
            {
                return Tuple.Create(false, "AccessToken is null");
            }

            return DoValidate();
        }

        protected virtual Tuple<bool, string> DoValidate()
        {
            return Tuple.Create(true, string.Empty);
        }


        public virtual T GetResponse()
        {
            this.PublicAccountInfo = AccessTokenFactory.Refresh(this.PublicAccountInfo);

            Tuple<bool, string> valudateResult = this.Validate();

            if (!valudateResult.Item1)
                throw new WxException(valudateResult.Item2);

            return DoResponse();
        }

        protected abstract T DoResponse();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="GetCompleted"></param>
        public void GetResponseAsync(Action<T, Exception> GetCompleted)
        {
            if (GetCompleted == null)
                throw new ArgumentNullException("GetComplete");

            Task.Factory.StartNew<T>(() => { return this.GetResponse(); })
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Exception e = task.Exception.Flatten().InnerException.GetBaseException();
                        logger.Error(e);

                        GetCompleted(null, e);
                    }
                    else
                    {
                        GetCompleted(task.Result, null);
                    }
                });
        }


        public T Deserialize(string json)
        {
            T o = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

            if (!string.IsNullOrEmpty(o.Errcode) && o.Errcode != "0")
            {
                string errorMessage = ApiErrors.GetError(o.Errcode);
                throw new WxException(o.Errcode, errorMessage);
            }

            o.JsonString = json;

            return o;
        }

        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        protected T HttpGet(string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = this.Url;
            }
            using (HttpWebClient client = new HttpWebClient())
            {
                string json = client.DownloadString(url);
                this.OnHttpGetting(client);
                this.logger.Trace(json);

                T o = null;

                try
                {
                    o = this.Deserialize(json);
                }
                catch (WxException we)
                {
                    if (we.ErrorCode == "40014")
                    {
                        this.PublicAccountInfo = AccessTokenFactory.RegetAccessToken(this.PublicAccountInfo);

                        json = client.DownloadString(url);

                        o = this.Deserialize(json);

                    }
                    else
                    {
                        this.logger.Error(we);
                        throw;
                    }

                }

                return o;
            }
        }

        protected T HttpPost(string inJson, string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = this.Url;
            }

            using (HttpWebClient client = new HttpWebClient())
            {
                this.OnHttpPosting(client);

                string json = client.UploadString(url, inJson);

                this.logger.Trace(json);

                T o = null;

                try
                {
                    o = this.Deserialize(json);
                }
                catch (WxException we)
                {
                    if (we.ErrorCode == "40014")
                    {
                        this.PublicAccountInfo = AccessTokenFactory.RegetAccessToken(this.PublicAccountInfo);

                        json = client.UploadString(url, inJson);

                        o = this.Deserialize(json);

                    }
                    else
                    {
                        this.logger.Error(we);
                        throw;
                    }

                }

                return o;
            }
        }

        protected virtual void OnHttpPosting(HttpWebClient webClient)
        {

        }

        protected virtual void OnHttpGetting(HttpWebClient webClient)
        {

        }



        object IRequest.GetResponse()
        {
            return this.GetResponse();
        }
    }
}

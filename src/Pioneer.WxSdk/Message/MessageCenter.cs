using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pioneer.WxSdk.Message
{
    /// <summary>
    /// 消息监听器
    /// </summary>
    public class MessageCenter
    {
        static ILog logger = LogFactory.GetLogger<MessageCenter>();

        static Type ListererType;

        static bool Inited = false;

        internal static void RegisterListener(IMessageListener listerer)
        {
            ListererType = listerer.GetType();
            Inited = true;
            logger.Info("注册监听器完成,监听器类型:" + listerer.GetType().FullName);
        }

        /// <summary>
        /// 异步消息处理
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="processCompleted"></param>
        public async  static Task ProcessRequestAsync(string url, string HttpMethod, Stream stream, Action<string, Exception> processCompleted)
        {
            if (processCompleted == null)
            {
                throw new ArgumentNullException("processCompleted");
            }

            await Task.Run<string>(() =>
            {
                return new MessageCenter().ProcessRequest(url, HttpMethod, stream);

            }).ContinueWith(task =>
            {

                if (task.IsFaulted)
                {
                    Exception e = null;
                    e = task.Exception.Flatten().InnerException;
                    logger.Error(e);
                    processCompleted(null, e);
                }
                else
                {
                    processCompleted(task.Result, null);
                }
            });
        }



        /// <summary>
        /// 同步消息处理
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string ProcessRequest(string url, string HttpMethod, Stream stream)
        {

            if (!Inited)
                throw new WxException("MessageCenter 尚未注册监听器 无法处理消息");

            if (SdkSetup.MessageTokenGetter == null)
                throw new ArgumentNullException("TokenGetter");

            var urlParmteres = QueryString(url);

            PublicAccount pi = SdkSetup.MessageTokenGetter(urlParmteres);
            string token = pi.MessageToken;

            if (HttpMethod == "GET")
            {
                return GetValidate(urlParmteres, token);
            }
            else if (HttpMethod == "POST")
            {
                if (!PostValidate(urlParmteres, token))
                {
                    throw new WxException("Token错误");
                }
            }
            else
            {
                throw new WxException("不支持的Http请求方式");
            }

            string encrypttype = urlParmteres["encrypt_type"];

            encrypttype = string.IsNullOrEmpty(encrypttype) ? "raw" : encrypttype.ToLower();

            RequestMessage req = MessageSerializer.DeSerialize(stream, encrypttype, pi);

            IMessageListener listener = Activator.CreateInstance(ListererType) as IMessageListener;

            ResponseMessage rsp = null;

            try
            {
                rsp = listener.ProcessRequest(req);
            }
            catch (Exception e)
            {
                logger.Error(e);

                throw;
            }

            if (rsp == null)
                return string.Empty;

            listener.Dispose();

            string rexml = rsp.ToXml();

            if (encrypttype == "aes")
            {
                string timestamp = urlParmteres["timestamp"];
                string nonce = urlParmteres["nonce"];
                MessageCryptor mc = new MessageCryptor(pi.MessageToken, pi.EncryptionKey, pi.AppId);
                rexml = mc.Encrypt(rexml, timestamp, nonce);
            }

            return rexml;
        }



        public static string GetValidate(IDictionary<string, string> request, string messageToken)
        {
            string signature = request["signature"];

            if (string.IsNullOrEmpty(signature))
            {
                return string.Empty;
            }

            string timestamp = request["timestamp"];
            string nonce = request["nonce"];
            string Echostr = request["echostr"];

            if (IsValidate(signature, timestamp, nonce, messageToken))
            {
                return Echostr;
            }
            return string.Empty;
        }

        public static bool PostValidate(IDictionary<string, string> request, string token)
        {
            string signature = request["signature"];
            string timestamp = request["timestamp"];
            string nonce = request["nonce"];
            return IsValidate(signature, timestamp, nonce, token);
        }

        public static string QueryString(string url, string key)
        {
            IDictionary<String, string> dic = QueryString(url);

            string value;

            dic.TryGetValue(key, out value);

            return value;
        }

        static IDictionary<string, string> QueryString(string url)
        {
            string query = url.Substring(url.IndexOf('?') + 1);

            string[] queryItems = query.Split('&');

            IDictionary<string, string> dic = new StringKeyValueCollection();

            foreach (var qi in queryItems)
            {
                string[] qitem = qi.Split('=');

                if (qitem.Length == 2)
                {
                    dic.Add(qitem[0], qitem[1]);
                }
                else
                {
                    logger.Warning(qi);
                }
            }

            if (dic.Count != queryItems.Length)
            {
                logger.Warning(url);
                logger.Warning(query);
            }

            return dic;
        }



        public static bool IsValidate(string signature, string timestamp, string nonce, string token)
        {
            return signature == GetSignature(timestamp, nonce, token);
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce, string token)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }
    }
}

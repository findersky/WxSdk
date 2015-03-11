using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Netsharp.Core;

namespace Pioneer.WxSdk
{
    public class WxMpClient
    {
        ILog logger;
        public WxMpClient()
        {
           this.logger = LogFactory.GetLogger(this.GetType());
        }

        public WxMpClient(WxPublicUser user)
        {
            this.CurrentUser = user;
        }

        WxPublicUser _currentUser;

        WxPublicUser CurrentUser
        {
            get
            {
                if (_currentUser == null || !_currentUser.IsLogin())
                {
                    throw new Exception("请先登录公众平台!");
                }
                return _currentUser;
            }
            set
            {
                if (value == null || !value.IsLogin())
                {
                    throw new Exception("请先登录公众平台!");
                }
                _currentUser = value;
            }
        }

        /// <summary>
        /// 默认的浏览器设置
        /// </summary>
        const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";

        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="str">密码</param>
        /// <returns>MD5处理后的密码</returns>
        static string Md5(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            byte[] buf = Encoding.Default.GetBytes(str);
            byte[] data = md5Hasher.ComputeHash(buf);

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString().ToLower();
        }


        /// <summary>
        /// 登陆函数
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="imgCode">图片验证码，如果没有可以为空串string.Empty</param>
        /// <returns>将得到的登陆结果全部放到WxPublicUser</returns>
        public WxPublicUser Login(string loginName, string password, string imgCode = null)
        {
            WxPublicUser pubUser = new WxPublicUser();// return result
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                throw new Exception("用户名或者密码不能为空");
            }
            password = Md5(password);
            string padata = "username=" + loginName + "&pwd=" + password + "&imgcode=" + imgCode + "&f=json";
            string url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";//请求登录的URL

            byte[] byteArray = Encoding.UTF8.GetBytes(padata);

            HttpWebClient client = new HttpWebClient();
            HttpRequestMessage rm = new HttpRequestMessage();
            rm.Uri = url;
            rm.Body = byteArray;
            rm.HttpMethod = HttpMethods.POST;
            rm.Headers.Add(HttpHeadKeys.Referer, "https://mp.weixin.qq.com/");

            byte[] data = client.UploadData(rm);
            string res = client.Encoding.GetString(data);

            WxLoginRetInfo retinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WxLoginRetInfo>(res);
            pubUser.LoginRetInfo = retinfo;

            if (retinfo.Base_resp.Ret != 0)
            {
                throw new Exception("登陆失败");
            }
            pubUser.Token = retinfo.Redirect_url.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得令牌;

            foreach (Cookie cookie in client.CookieContainer.GetCookies(new Uri(url)))
            {
                if (cookie.Name == "slave_user")
                {
                    pubUser.User_name = cookie.Value;
                }
            }

            HttpRequestMessage homeRequest = new HttpRequestMessage();
            homeRequest.Uri = "https://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=" + pubUser.Token;
            homeRequest.HttpMethod = HttpMethods.GET;
            homeRequest.Headers.Add(HttpHeadKeys.Referer, "https://mp.weixin.qq.com/cgi-bin/settingpage?t=setting/index&action=index&token=" + pubUser.Token + "&lang=zh_CN");

            string pageSource = client.DownloadString(homeRequest);

            pubUser.Ticket = RegexPageSource(pageSource, "ticket:\"\\w*\"");
            if (string.IsNullOrEmpty(pubUser.User_name))
                pubUser.User_name = RegexPageSource(pageSource, "user_name:\"\\w*\"");
            pubUser.Lang = "zh_CN";
            CurrentUser = pubUser;
            pubUser.LoginCookie = client.CookieContainer;
            return pubUser;
        }

        string RegexPageSource(string PageSource, string RegExpression)
        {
            Match matcher = Regex.Match(PageSource, RegExpression, RegexOptions.IgnoreCase);
            if (matcher.Success)
            {
                return matcher.Groups[0].Value.Substring(matcher.Groups[0].Value.IndexOf('"') + 1).TrimEnd('"');
            }
            return null;
        }


        public int UpCoverImg(string filePath, byte[] imageData, out string ImgID, out string retMsg)
        {
            return this.UpCoverImg(this.CurrentUser, filePath, imageData, out ImgID, out retMsg);
        }


        /// <summary>
        /// 上传封面图片
        /// </summary>
        /// <param name="pubUser">登陆后的用户信息</param>
        /// <param name="filePath">图片文件路径</param>
        /// <param name="ImgID">上传成功后得到ID</param>
        /// <param name="retMsg">上传后的返回消息</param>
        /// <returns>上传后的返回码</returns>
        public int UpCoverImg(WxPublicUser pubUser, string imageName, byte[] imageData, out string ImgID, out string retMsg)
        {
            if (!imageName.EndsWith(".jpg"))
            {
                imageName += ".jpg";
            }


            //上传封面图片的请求格式
            //https://mp.weixin.qq.com/cgi-bin/filetransfer?
            //action=upload_material&
            //f=json&
            //ticket_id=xxxx&
            //ticket=d97d13c9abb09aca1c4761aac5f9d0e4dd5145c3&
            //token=1835487172&
            //lang=zh_CN
            string url = "https://mp.weixin.qq.com/cgi-bin/filetransfer?action=upload_material&f=json&ticket_id=" + pubUser.User_name
                            + "&ticket=" + pubUser.Ticket
                            + "&token=" + pubUser.Token
                            + "&lang=zh_CN";
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.UserAgent = UserAgent;
            wr.CookieContainer = pubUser.LoginCookie;
            //Referer的格式：https://mp.weixin.qq.com/cgi-bin/appmsg?t=media/appmsg_edit&action=edit&type=10&isMul=1&isNew=1&lang=zh_CN&token=1835487172
            wr.Referer = "https://mp.weixin.qq.com/cgi-bin/appmsg?t=media/appmsg_edit&action=edit&type=10&isMul=1&isNew=1&lang=zh_CN&token=" + pubUser.Token;

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("Filename", imageName);
            nvc.Add("folder", "/cgi-bin/uploads");
            Stream rs = wr.GetRequestStream();
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            string formitem = null;
            byte[] formitembytes = null;
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                formitem = string.Format(formdataTemplate, key, nvc[key]);
                formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string paramName = "file";
            string contentType = "application/octet-stream";
            string header = string.Format(headerTemplate, paramName, imageName, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            //FileStream fileStream = new FileStream(imgFile.FullName, FileMode.Open, FileAccess.Read);

            MemoryStream fileStream = new MemoryStream(imageData);
            fileStream.Position = 0;

            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            formitem = string.Format(formdataTemplate, "Upload", "Submit Query");
            formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            rs.Write(formitembytes, 0, formitembytes.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            HttpWebResponse wresp = null;
            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                StreamReader sr = new StreamReader(wresp.GetResponseStream(), Encoding.Default);
                string retInfo = sr.ReadToEnd();


                this.logger.Trace("UpCoverImg" + Environment.NewLine + retInfo);


                JToken token = JObject.Parse(retInfo);
                ImgID = (string)token.SelectToken("content");
                retMsg = retInfo;
                return (int)token.SelectToken("base_resp").SelectToken("ret");
            }
            catch (Exception ex)
            {
                retMsg = ex.ToString();

                this.logger.Error(ex);

                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                ImgID = null;
                return -1;
            }
            finally
            {
                wr = null;
            }
        }


        public int UpMessages(List<WxMessage> messages, out string retinfo)
        {
            return this.UpMessages(this.CurrentUser, messages, out retinfo);
        }

        /// <summary>
        /// 上传多图文信息
        /// </summary>
        /// <param name="pubUser">登陆后的用户信息</param>
        /// <param name="msgs">要上传的多图文信息列表</param>
        /// <param name="retInfo">上传后的返回信息</param>
        /// <returns>0为正常</returns>
        public int UpMessages(WxPublicUser pubUser, List<WxMessage> msgs, out string retInfo)
        {
            if (!CheckMessages(msgs, out retInfo))
            {
                return -1;
            }

            //请求格式
            //POST https://mp.weixin.qq.com/cgi-bin/operate_appmsg HTTP/1.1
            //Accept: text/html, */*; q=0.01
            //Referer: https://mp.weixin.qq.com/cgi-bin/appmsg?t=media/appmsg_edit&action=edit&type=10&isMul=0&isNew=1&lang=zh_CN&token=369376755
            //Origin: https://mp.weixin.qq.com
            //X-Requested-With: XMLHttpRequest
            //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36
            //Content-Type: application/x-www-form-urlencoded; charset=UTF-8
            int isMul = (msgs.Count == 1) ? 0 : 1;
            string url = "https://mp.weixin.qq.com/cgi-bin/operate_appmsg";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, */*; q=0.01";
            request.Referer = "https://mp.weixin.qq.com/cgi-bin/appmsg?t=media/appmsg_edit&action=edit&type=10&isMul=" + isMul
                                + "&isNew=1&lang=zh_CN&token=" + pubUser.Token;
            request.UserAgent = UserAgent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.CookieContainer = pubUser.LoginCookie;


            //ajax:1
            //token:369376755
            //lang:zh_CN
            //random:0.0679663096088916
            //f:json
            //t:ajax-response
            //sub:create
            //type:10
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("AppMsgId", "");
            nvc.Add("count", msgs.Count.ToString());
            nvc.Add("ajax", "1");
            nvc.Add("token", pubUser.Token);
            nvc.Add("lang", pubUser.Lang);
            nvc.Add("random", new Random().NextDouble().ToString());
            nvc.Add("f", "json");
            nvc.Add("t", "ajax-response");
            nvc.Add("sub", "create");
            nvc.Add("type", "10");

            var parameters = new StringBuilder();
            foreach (string key in nvc.Keys)
            {
                parameters.AppendFormat("{0}={1}&",
                    HttpUtility.UrlEncode(key),
                    HttpUtility.UrlEncode(nvc[key]));
            }

            for (int i = 0; i < msgs.Count; i++)
            {
                //每个图文信息的内容：
                //title0:123
                //content0:<p>123</p><p><br/></p><p>12312</p><p>1231</p>
                //digest0:digestskdj
                //author0:
                //fileid0:10000067
                //show_cover_pic0:1
                //sourceurl0:

                parameters.AppendFormat("title{0}={1}&", i.ToString(), HttpUtility.UrlEncode(msgs[i].Title));
                parameters.AppendFormat("content{0}={1}&", i.ToString(), HttpUtility.UrlEncode(msgs[i].Content));
                parameters.AppendFormat("digest{0}={1}&", i.ToString(), HttpUtility.UrlEncode(msgs[i].Digest));
                parameters.AppendFormat("author{0}={1}&", i.ToString(), HttpUtility.UrlEncode(msgs[i].Author));
                parameters.AppendFormat("fileid{0}={1}&", i.ToString(), HttpUtility.UrlEncode(msgs[i].ConverMediaId));
                parameters.AppendFormat("show_cover_pic{0}={1}&", i.ToString(), HttpUtility.UrlEncode(msgs[i].ShowCoverPic ? "1" : "0"));
                parameters.AppendFormat("sourceurl{0}={1}&", i.ToString(), HttpUtility.UrlEncode(msgs[i].SourceUrl));
            }
            parameters.Length -= 1;//remove the last &

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(parameters.ToString());
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            retInfo = sr.ReadToEnd();

            this.logger.Info("uploadmsg" + Environment.NewLine + retInfo);

            //{"ret":"0", "msg":"OK"}
            JToken jToken = JObject.Parse(retInfo);
            return (int)jToken.SelectToken("ret");
        }


        public WxMsgListRetInfo GetMessages()
        {
            return this.GetMessages(CurrentUser);
        }

        /// <summary>
        /// 查看现在拥有的多图文信息，主要是为了得到信息的appmsgid
        /// </summary>
        /// <param name="pubUser">登陆后的用户信息</param>
        /// <returns>WxMsgListRetInfo包含了请求返回信息和多图文信息</returns>
        public WxMsgListRetInfo GetMessages(WxPublicUser pubUser)
        {
            //请求格式
            //GET /cgi-bin/appmsg?token=1269820950&lang=zh_CN&random=0.9757365568075329&f=json&ajax=1&type=10&action=list&begin=0&count=10 HTTP/1.1
            //Host: mp.weixin.qq.com
            //Connection: keep-alive
            //Accept: application/json, text/javascript, */*; q=0.01
            //X-Requested-With: XMLHttpRequest
            //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36
            //Referer: https://mp.weixin.qq.com/cgi-bin/masssendpage?t=mass/send&token=1269820950&lang=zh_CN
            //Accept-Encoding: gzip,deflate,sdch
            //Accept-Language: zh-CN,zh;q=0.8

            string url = string.Format("https://mp.weixin.qq.com/cgi-bin/appmsg?token={0}&lang={1}&random={2}&f=json&ajax=1&type=10&action=list&begin=0&count=10",
                                        pubUser.Token,
                                        pubUser.Lang,
                                        new Random().NextDouble().ToString());
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = "GET";
            wr.CookieContainer = pubUser.LoginCookie;
            wr.Accept = "application/json, text/javascript, */*; q=0.01";
            wr.UserAgent = UserAgent;
            wr.Referer = string.Format("https://mp.weixin.qq.com/cgi-bin/masssendpage?t=mass/send&token={0}&lang={1}", pubUser.Token, pubUser.Lang);
            HttpWebResponse rp = (HttpWebResponse)wr.GetResponse();
            string retInfo = new StreamReader(rp.GetResponseStream(), Encoding.UTF8).ReadToEnd();

            this.logger.Info("msgList" + Environment.NewLine + retInfo);

            WxMsgListRetInfo msgList = Newtonsoft.Json.JsonConvert.DeserializeObject<WxMsgListRetInfo>(retInfo);
            return msgList;
        }


        public int SendMessage(string AppMsgId, out string retMsg)
        {
            return SendMessage(this.CurrentUser, AppMsgId, out retMsg);
        }
        /// <summary>
        /// 群发多图文信息
        /// </summary>
        /// <param name="pubUser">登陆后的用户信息</param>
        /// <param name="AppMsgId">要群发的多图文信息的appmsgid</param>
        /// <param name="retMsg">请求后的返回信息</param>
        /// <returns>请求后的返回码</returns>
        public int SendMessage(WxPublicUser pubUser, string AppMsgId, out string retMsg)
        {
            //请求格式
            //POST https://mp.weixin.qq.com/cgi-bin/masssend HTTP/1.1
            //Accept: text/html, */*; q=0.01
            //Referer: https://mp.weixin.qq.com/cgi-bin/masssendpage?t=mass/send&token=1269820950&lang=zh_CN
            //Origin: https://mp.weixin.qq.com
            //X-Requested-With: XMLHttpRequest
            //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36
            //Content-Type: application/x-www-form-urlencoded; charset=UTF-8

            string url = "https://mp.weixin.qq.com/cgi-bin/masssend";
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = "POST";
            wr.CookieContainer = pubUser.LoginCookie;
            wr.Accept = "text/html, */*; q=0.01";
            wr.Referer = string.Format("https://mp.weixin.qq.com/cgi-bin/masssendpage?t=mass/send&token={0}&lang={1}", pubUser.Token, pubUser.Lang);
            wr.UserAgent = UserAgent;
            wr.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            //type:10
            //appmsgid:10000077
            //sex:0
            //groupid:-1
            //synctxweibo:0
            //synctxnews:0
            //country:
            //province:
            //city:
            //imgcode:
            //token:1269820950
            //lang:zh_CN
            //random:0.1450110177975148
            //f:json
            //ajax:1
            //t:ajax-response

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("type", "10");
            nvc.Add("appmsgid", AppMsgId);
            nvc.Add("sex", "0");
            nvc.Add("groupid", "-1");
            nvc.Add("synctxweibo", "0");
            nvc.Add("synctxnews", "0");
            nvc.Add("country", string.Empty);
            nvc.Add("province", string.Empty);
            nvc.Add("city", string.Empty);
            nvc.Add("imgcode", string.Empty);
            nvc.Add("token", pubUser.Token);
            nvc.Add("lang", pubUser.Lang);
            nvc.Add("random", new Random().NextDouble().ToString());
            nvc.Add("f", "json");
            nvc.Add("ajax", "1");
            nvc.Add("t", "ajax-response");

            var parameters = new StringBuilder();
            foreach (string key in nvc.Keys)
            {
                parameters.AppendFormat("{0}={1}&",
                    HttpUtility.UrlEncode(key),
                    HttpUtility.UrlEncode(nvc[key]));
            }
            parameters.Length -= 1;//remove the last &

            using (var writer = new StreamWriter(wr.GetRequestStream()))
            {
                writer.Write(parameters.ToString());
            }

            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string retInfo = sr.ReadToEnd();
            this.logger.Trace("发送消息Id:" + AppMsgId);
            this.logger.Trace("SendMessage" + Environment.NewLine + retInfo);

            JToken jToken = JObject.Parse(retInfo);
            retMsg = retInfo;//(string)jToken.SelectToken("msg");
            return (int)jToken.SelectToken("ret");

            //微信页面上的javascript代码
            //"{\"ret\":\"64004\", \"msg\":\"not have masssend quota today!\"}"
            //if (r.ret && r.ret == "0") {
            //  i.suc("发送成功"), t && t(r);
            //  return;
            //}
            //r.ret && r.ret == "64004" ? i.err("今天的群发数量已到，无法群发") : 
            //r.ret && r.ret == "67008" ? i.err("消息中可能含有具备安全风险的链接，请检查") : 
            //r.ret == "-6" ? i.err("请输入验证码") : i.err("发送失败"), n && n(r);
        }


        public bool UpContextImage(string filePath, out string imageUrl, out string retInfo)
        {
            return this.UpContextImage(this.CurrentUser, filePath, out imageUrl, out retInfo);
        }

        /// <summary>
        /// 上传正文中的图片
        /// </summary>
        /// <param name="pubUser">登陆后的用户信息</param>
        /// <param name="filePath">图片的物理路径</param>
        /// <param name="ImgUrl">上传成功后得到的URL</param>
        /// <param name="retInfo">请求后的返回信息</param>
        /// <returns>请求后的返回码</returns>
        public bool UpContextImage(WxPublicUser pubUser, string filePath, out string ImgUrl, out string retInfo)
        {
            FileInfo imgFile = new FileInfo(filePath);
            if (!imgFile.Exists)
            {
                ImgUrl = null;
                retInfo = "正文的图片不存在！请重新上传！";
                return false;
            }
            //POST /cgi-bin/uploadimg2cdn?t=ajax-editor-upload-img&lang=zh_CN&token=1121369994 HTTP/1.1
            //Host: mp.weixin.qq.com
            //Connection: keep-alive
            //Content-Length: 21235
            //Cache-Control: no-cache
            //Origin: https://mp.weixin.qq.com
            //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36
            //Content-Type: multipart/form-data; boundary=nccgxlrtxqcybakdfjgdfnuknabodhvn
            //Accept: */*
            //Referer: https://mp.weixin.qq.com/mpres/htmledition/ueditor/dialogs/image/image.html
            //Accept-Encoding: gzip,deflate,sdch
            //Accept-Language: zh-CN,zh;q=0.8

            string url = string.Format("https://mp.weixin.qq.com/cgi-bin/uploadimg2cdn?t=ajax-editor-upload-img&lang={0}&token={1}", pubUser.Lang, pubUser.Token);
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.UserAgent = UserAgent;
            wr.CookieContainer = pubUser.LoginCookie;
            wr.Referer = "https://mp.weixin.qq.com/mpres/htmledition/ueditor/dialogs/image/image.html";

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("Filename", imgFile.Name);
            nvc.Add("fileName", imgFile.Name);
            nvc.Add("pictitle", imgFile.Name);//TODO
            nvc.Add("param2", "value2");
            nvc.Add("param1", "value1");
            Stream rs = wr.GetRequestStream();
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            string formitem = null;
            byte[] formitembytes = null;
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                formitem = string.Format(formdataTemplate, key, nvc[key]);
                formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }


            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string paramName = "upfile";
            string contentType = "application/octet-stream";
            string header = string.Format(headerTemplate, paramName, imgFile.Name, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(imgFile.FullName, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            formitem = string.Format(formdataTemplate, "Upload", "Submit Query");
            formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            rs.Write(formitembytes, 0, formitembytes.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            HttpWebResponse wresp = null;
            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                StreamReader sr = new StreamReader(wresp.GetResponseStream(), Encoding.Default);
                retInfo = sr.ReadToEnd();

                JToken token = JObject.Parse(retInfo);
                //string state = (string)token.SelectToken("state");
                ImgUrl = (string)token.SelectToken("url");

                return true;
            }
            catch (Exception ex)
            {
                retInfo = ex.ToString();
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                ImgUrl = null;
                return false;
            }
            finally
            {
                wr = null;
            }
        }

        /// <summary>
        /// 上传多图文信息前检查相关格式
        /// </summary>
        /// <param name="msgs">多图文信息列表</param>
        /// <param name="ErrMsg">检查结果</param>
        /// <returns>通过则true，否则false</returns>
        bool CheckMessages(List<WxMessage> msgs, out string ErrMsg)
        {
            if (msgs.Count < 1)
            {
                ErrMsg = "发送的正文信息的数量少于1";//TODO:hard code
                return false;
            }


            foreach (var msg in msgs)
            {
                if (msg.Title == null ||
                    msg.Title.Trim() == string.Empty ||
                    msg.ConverMediaId == null ||
                    msg.ConverMediaId.Trim() == string.Empty ||
                    msg.Content == null ||
                    msg.Content.Trim() == string.Empty
                    )
                {
                    ErrMsg = "每个图文信息都必须要有：“标题”、“封面图片”和“正文”";
                    return false;
                }
            }
            if (msgs.Count > 8)
            {
                ErrMsg = "最多只能同时发8条图文信息";
                return false;
            }

            ErrMsg = string.Empty;
            return true;
        }

    }
}

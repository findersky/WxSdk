using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    public class JsTicketInfo
    {
        public string JsTicket
        {
            get;
            set;
        }

        public DateTime LastUpdateTime
        {
            get;
            set;
        }

        public bool IsExpiry
        {
            get
            {
                if ((DateTime.Now - LastUpdateTime).TotalMinutes > 100)
                {
                    return true;
                }

                return false;
            }
        }
    }

    public class JsTicketRequest : Request<JsTicketResponse>
    {
        public static ConcurrentDictionary<string, JsTicketInfo> JsTickets = new ConcurrentDictionary<string, JsTicketInfo>();

        public override string Url
        {
            get
            {
                return string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", this.AccessToken);
            }
        }


        public IDictionary<string, string> SignItems
        {
            get;
            set;
        }


        public string Sign()
        {

            if (this.PublicAccountInfo == null)
                throw new ArgumentNullException("PublicAccountInfo");

            if (this.SignItems == null || this.SignItems.Count == 0)
            {
                throw new ArgumentNullException("SignItems");
            }

            JsTicketInfo jstinfo;

            if (!JsTickets.TryGetValue(this.PublicAccountInfo.AppId, out jstinfo))
            {
                var rsp = this.GetResponse();

                jstinfo = new JsTicketInfo();
                jstinfo.LastUpdateTime = DateTime.Now;
                jstinfo.JsTicket = rsp.Ticket;

                JsTickets.TryAdd(this.PublicAccountInfo.AppId, jstinfo);
            }

            if (jstinfo.IsExpiry)
            {
                var rsp = this.GetResponse();

                jstinfo.JsTicket = rsp.Ticket;
                jstinfo.LastUpdateTime = DateTime.Now;
                JsTickets[this.PublicAccountInfo.AppId] = jstinfo;
            }


            this.SignItems.Remove("jsapi_ticket");
            this.SignItems.Add("jsapi_ticket", jstinfo.JsTicket);

            return Sha1(this.SignItems);

        }


        string Sha1(IDictionary<string, string> dic)
        {
            SortedDictionary<string, string> items = new SortedDictionary<string, string>(dic);

            List<string> sb = new List<string>();
            foreach (var x in items)
            {
                if (string.IsNullOrEmpty(x.Value))
                    continue;

                sb.Add(x.Key.Trim().ToLower() + "=" + x.Value.Trim());
            }

            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(string.Join("&", sb.ToArray())));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }


        protected override JsTicketResponse DoResponse()
        {
            return this.HttpGet();
        }
    }
}

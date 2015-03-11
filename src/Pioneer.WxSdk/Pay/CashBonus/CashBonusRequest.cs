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
    [Apipath("现金红包","/pay/CashBonus")]
    public class CashBonusRequest : PayCertificateRequest<CashBonusResponse>
    {

        protected override string Url
        {
            get { return "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack"; }
        }

        protected override void DoValidate()
        {
            if (this.Info == null)
                throw new ArgumentNullException("CashBonusInfo");

            SdkHelper.Validate(this.Info);

            if (string.IsNullOrEmpty(this.CretPath))
            {
                throw new ArgumentNullException("Cretpath");
            }

            base.DoValidate();
        }



        protected override CashBonusResponse DoResponse()
        {

            StringKeyValueCollection sk = new StringKeyValueCollection();

            sk.Add("mch_billno", this.Info.OrderCode);
            sk.Add("mch_id", this.MchId);
            sk.Add("nonce_str", PayHelp.NonceString);
            sk.Add("wxappid", this.AppId);
            sk.Add("nick_name", this.Info.NickName);
            sk.Add("send_name", this.Info.NickName);
            sk.Add("re_openid", this.Info.OpenId);
            sk.Add("total_amount", GetAmountValue(this.Info.Amount).ToString());
            sk.Add("min_value", GetAmountValue(this.Info.Amount).ToString());
            sk.Add("max_value", GetAmountValue(this.Info.Amount).ToString());
            sk.Add("total_num", "1");
            sk.Add("wishing", this.Info.Wishing);
            sk.Add("client_ip", GetIp());
            sk.Add("act_name", this.Info.ActiveName);
            sk.Add("remark", this.Info.ReMark);
            sk.Add("logo_imgurl", this.Info.LogoUrl);
            sk.Add("share_content", this.Info.ShareContent);
            sk.Add("share_url", this.Info.ShareUrl);
            sk.Add("share_imgurl", this.Info.ShareImage);

            Sign(sk);

            this.CertificateNo = this.CretPath;


            return this.HttpPost(sk);

        }


        string GetIp()
        {
            Uri u = new Uri(this.Info.Url);

            var i = Dns.GetHostEntry(u.Host);
            return i.AddressList[0].ToString();
        }

        int GetAmountValue(decimal d)
        {
            return (int)decimal.Floor(d * 100);
        }

        public string CretPath
        {
            get;
            set;
        }

        public CashBonusInfo Info
        {
            get;
            set;
        }


    }
}

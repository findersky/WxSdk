using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    public enum PublicAccountType
    {
        Subscription, Service, Enterprise
    }


    /// <summary>
    /// 公众号信息
    /// </summary>
    public class PublicAccount
    {
        public PublicAccount()
        {
            this.LastUpdateTime = DateTime.Now;
        }


        public bool Certified
        {
            get;
            set;
        }


        public PublicAccountType PublicAccountType
        {
            get;
            set;
        }

        /// <summary>
        /// 微信公众号设置配置的消息Token
        /// </summary>
        public string MessageToken
        {
            get;
            set;
        }

        public string AppId
        {
            get;
            set;
        }

        public string AppSecret
        {
            get;
            set;
        }

        /// <summary>
        /// 即消息中的FromUserName
        /// </summary>
        public string OriginalId
        {
            get;
            set;
        }

        public string EncryptionKey
        {
            get;
            set;
        }

        public DateTime? LastUpdateTime
        {
            get;
            set;
        }

        public object Tag
        {
            get;
            set;
        }

        public string AccessToken
        {
            get;
            set;
        }


        public bool IsExpired
        {
            get
            {
                if (string.IsNullOrEmpty(AccessToken))
                    return true;

                if (!LastUpdateTime.HasValue)
                    return true;
                TimeSpan ts = DateTime.Now - LastUpdateTime.Value;
                if (ts.TotalMinutes > 90)
                {
                    return true;
                }
                return false;
            }
        }


        public static bool operator ==(PublicAccount lvalue, PublicAccount rvalue)
        {
            if (object.Equals(lvalue, null) && !object.Equals(rvalue, null))
            {
                return false;
            }

            if (!object.Equals(lvalue, null) && object.Equals(rvalue, null))
            {
                return false;
            }

            if (object.Equals(lvalue, null) && object.Equals(rvalue, null))
            {
                return true;
            }



            if (lvalue.OriginalId != rvalue.OriginalId)
                return false;

            if (lvalue.AppId != rvalue.AppId)
                return false;

            if (lvalue.AppSecret != rvalue.AppSecret)
                return false;

            if (lvalue.AccessToken != rvalue.AccessToken)
                return false;

            return true;
        }

        public static bool operator !=(PublicAccount lvalue, PublicAccount rvalue)
        {
            return !(lvalue == rvalue);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}

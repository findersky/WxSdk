using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    public class CashBonusInfo
    {
        [NotNull]
        [StringMaxLength(28)]
        public string OrderCode
        {
            get;
            set;
        }
        [NotNull]
        public string OpenId
        {
            get;
            set;
        }
        [NotNull]
        public decimal Amount
        {
            get;
            set;
        }
        [NotNull]
        public string Url
        {
            get;
            set;
        }
        [NotNull]
        [StringMaxLength(32)]
        public string SendName
        {
            get;
            set;
        }
        [NotNull]
        [StringMaxLength(32)]
        public string NickName
        {
            get;
            set;
        }
        [NotNull]
        [StringMaxLength(256)]
        public string ReMark
        {
            get;
            set;
        }

        public decimal Min
        {
            get;
            set;
        }

        public decimal Max
        {
            get;
            set;
        }
        [NotNull]
        [StringMaxLength(32)]
        public string ActiveName
        {
            get;
            set;
        }
        [NotNull]
        [StringMaxLength(128)]
        public string Wishing
        {
            get;
            set;
        }

        [StringMaxLength(128)]
        public string LogoUrl
        {
            get;
            set;
        }

        [StringMaxLength(256)]
        public string ShareContent
        {
            get;
            set;
        }
        [StringMaxLength(128)]
        public string ShareUrl
        {
            get;
            set;
        }

        [StringMaxLength(128)]
        public string ShareImage
        {
            get;
            set;
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Pay
{
    [XmlRoot("xml")]
    public class PayWarningResponse
    {
        public string AppId
        {
            get;
            set;
        }

        public string ErrorType
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string AlarmContent
        {
            get;
            set;
        }

        public string TimeStamp
        {
            get;
            set;
        }

        public string AppSignature
        {
            get;
            set;
        }

        public string SignMothed
        {
            get;
            set;
        }

        /// <summary>
        /// 发货超时时  返回交易Id
        /// </summary>
        public string TransId
        {
            get
            {
                if (this.ErrorType == "1001")
                {
                    return this.AlarmContent.Replace("transaation_id=", "");
                }

                return string.Empty;
            }
        }
    }
}

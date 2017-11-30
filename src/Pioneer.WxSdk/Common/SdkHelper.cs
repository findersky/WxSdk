using System;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    public static class SdkHelper
    {

        public static DateTime BaseTime = new DateTime(1970, 1, 1);//Unix起始时间

        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime ToDateTime(long dateTimeFromXml)
        {
            return BaseTime.AddTicks((dateTimeFromXml + 8 * 60 * 60) * 10000000);
        }
        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime ToDateTime(string dateTimeFromXml)
        {
            return ToDateTime(long.Parse(dateTimeFromXml));
        }

        /// <summary>
        /// 获取微信DateTime
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long ToUnixTime(DateTime dateTime)
        {
            return (dateTime.Ticks - BaseTime.Ticks) / 10000000 - 8 * 60 * 60;
        }


        static ILog logger = LogFactory.GetLogger(typeof(SdkHelper));


        public static void Validate(object obj)
        {
            var props = obj.GetType().GetProperties();

            foreach (var p in props)
            {
                if (p.IsDefined(typeof(NotNullAttribute), true))
                {
                    var o = p.GetValue(obj, null);

                    if (o == null)
                        throw new WxException(p.Name + " is null");

                }

                if (p.IsDefined(typeof(StringMaxLengthAttribute), true))
                {
                    object[] os = p.GetCustomAttributes(typeof(StringMaxLengthAttribute), true);

                    StringMaxLengthAttribute att = os[0] as StringMaxLengthAttribute;

                    string s = p.GetValue(obj, null) as string;

                    if (!string.IsNullOrEmpty(s))
                    {
                        if (Encoding.UTF8.GetByteCount(s) > att.MaxLength)
                        {
                            throw new WxException(p.Name + "长度超过" + att.MaxLength + "个字符 或者" + att.MaxLength / 3 + "个汉字");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 得到微信手机端浏览器
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static WxClientVersion GetWxClientVersion(string userAgent)
        {
            string agent = userAgent.ToLower();

            WxClientVersion version = new WxClientVersion();

            if (agent.Contains("micromessenger"))
            {
                version.IsWxBrowser = true;

                if (agent.Contains("iphone") || agent.Contains("apple"))
                {
                    version.PhoneOs = "Iphone";
                }
                else if (agent.Contains("android"))
                {
                    version.PhoneOs = "Android";
                }
                else
                {
                    version.PhoneOs = "unKnow";
                }

                string[] str = agent.Split(new string[] { "micromessenger/" }, StringSplitOptions.RemoveEmptyEntries);
                version.WxVersion = str[1];
                string[] versions = version.WxVersion.Split('.');
                if (versions.Any() == true)
                {
                    if (Int32.Parse(versions[0]) >= 5)
                        version.CanPay = true;
                }
            }
            return version;
        }
    }


    /// <summary>
    /// 微信客户端信息
    /// </summary>
    public class WxClientVersion
    {
        /// <summary>
        /// 是否微信浏览器
        /// </summary>
        public bool IsWxBrowser { get; set; }

        /// <summary>
        /// 手机客户端类型目前只有：Android,Iphone
        /// </summary>
        public string PhoneOs { get; set; }

        /// <summary>
        /// 是否支持微信支付
        /// </summary>
        public bool CanPay { get; set; }
        /// <summary>
        /// 手机客户端版本
        /// </summary>
        public string WxVersion { get; set; }
    }
}

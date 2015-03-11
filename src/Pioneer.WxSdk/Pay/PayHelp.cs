using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pioneer.WxSdk.Pay
{
    public class PayHelp
    {
        public static string Sign(IDictionary<string, string> keyValues, string key)
        {
            SortedDictionary<string, string> sdic = new SortedDictionary<string, string>(keyValues);

            List<string> sb = new List<string>();
            foreach (var x in sdic)
            {
                if (string.IsNullOrEmpty(x.Value))
                    continue;

                sb.Add(x.Key.Trim() + "=" + x.Value.Trim());
            }

            string sortedValues = string.Join("&", sb.ToArray());

            sortedValues = sortedValues + "&key=" + key;

            return MD5(sortedValues).ToUpper();
        }


        public static string MD5(string s)
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(s);

            MD5CryptoServiceProvider cryptHandler = new MD5CryptoServiceProvider();
            byte[] hash = cryptHandler.ComputeHash(textBytes);
            string ret = "";
            foreach (byte a in hash)
            {
                if (a < 16)
                    ret += "0" + a.ToString("x");
                else
                    ret += a.ToString("x");
            }
            return ret;
        }

        //public static string MD5(string s)
        //{
        //    char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        //            'A', 'B', 'C', 'D', 'E', 'F' };

        //    byte[] btInput = System.Text.Encoding.UTF8.GetBytes(s);
        //    // 获得MD5摘要算法的 MessageDigest 对象
        //    MD5 mdInst = System.Security.Cryptography.MD5.Create();
        //    // 使用指定的字节更新摘要
        //    mdInst.ComputeHash(btInput);
        //    // 获得密文
        //    byte[] md = mdInst.Hash;
        //    // 把密文转换成十六进制的字符串形式
        //    int j = md.Length;
        //    char[] str = new char[j * 2];
        //    int k = 0;
        //    for (int i = 0; i < j; i++)
        //    {
        //        byte byte0 = md[i];
        //        str[k++] = hexDigits[(int)(((byte)byte0) >> 4) & 0xf];
        //        str[k++] = hexDigits[byte0 & 0xf];
        //    }
        //    return new string(str);
        //}


        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string NonceString
        {
            get
            {
                String res = "";
                Random rd = new Random();
                for (int i = 0; i < 16; i++)
                {
                    res += chars[rd.Next(chars.Length - 1)];
                }
                return res;
            }
        }

        public static string Ts
        {
            get
            {
                return SdkHelper.ToUnixTime(DateTime.Now).ToString();
            }
        }

        /// <summary>
        /// 转换金额为分
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToWxPayAmount(decimal d)
        {
            if (d <= 0)
                throw new ArgumentException("金额超出合法范围");

            decimal fee = Math.Round(d, 2);

            int feeint = (int)(fee * 100);

            return feeint.ToString();
        }

    }


}

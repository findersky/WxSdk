using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Pioneer.WxSdk
{
    public static class XmlService
    {

        static ILog logger = LogFactory.GetLogger(typeof(XmlService));

        public static string WriteXml(IDictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter xw = XmlTextWriter.Create(sb, new XmlWriterSettings() { ConformanceLevel = ConformanceLevel.Auto }))
            {
                xw.WriteStartElement("xml");

                foreach (var xx in dic)
                {
                    xw.WriteStartElement(xx.Key);
                    xw.WriteCData(xx.Value);
                    xw.WriteEndElement();
                }

                xw.WriteEndElement();
                xw.Flush();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 反序列化一个Stream 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns>Tuple T object string 序列化错误是，返回原始的xml"/></returns>
        public static T FromStream<T>(Stream stream, Encoding encoding)
            where T : class,new()
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;

            if (encoding == null)
                encoding = Encoding.UTF8;

            string xml = string.Empty;

            using (StreamReader sr = new StreamReader(ms, encoding))
            {
                xml = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(xml))
                return null;

            return FromXml<T>(xml);
        }


        /// <summary>
        /// 反序列化一个xml string 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml)
            where T : class,new()
        {

            T t = null;

            StringReader sr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(sr);

            try
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));

                t = xs.Deserialize(xr) as T;
            }
            catch (Exception e)
            {
                logger.Error(e);
                logger.Error(xml);

                throw;
            }
            return t;
        }

        public static string ToXml(object o)
        {
            StringWriter sw = new StringWriter();
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(o.GetType());
            xs.Serialize(sw, o);
            return sw.ToString();
        }
    }
}

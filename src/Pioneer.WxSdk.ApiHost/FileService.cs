using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pioneer.WxSdk.ApiHost
{
    public enum SerializeType
    {
        Json, Xml
    }

    public class FileService
    {
        static string rootPath;
        static string RootPath
        {
            get
            {
                if (rootPath == null)
                {
                    string binPath = System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
                    string appbasePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                    if (binPath == null)
                    {
                        rootPath = appbasePath;
                    }
                    else
                    {
                        if (binPath.StartsWith(appbasePath))
                        {
                            rootPath = binPath;
                        }
                        else
                        {
                            rootPath = Path.Combine(appbasePath, binPath);
                        }
                    }

                    if (rootPath == null || !!File.Exists(rootPath))
                    {
                        throw new Exception("未能设置应用程序的根目录");
                    }
                }

                return rootPath;
            }
        }

        public static string WebRoot
        {
            get
            {
                return Path.Combine(RootPath, "Web");
            }
        }

        public static string ConfigPath
        {
            get
            {
                return Path.Combine(RootPath, "Config");
            }
        }


        public static string BinPath
        {
            get
            {
                return RootPath;
            }
        }

        public static string Parent = "..\\";

        /// <summary>
        /// 反序列化一个Stream 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns>Tuple T object string 序列化错误是，返回原始的xml"/></returns>
        public static T FromXml<T>(string xml)
            where T : class,new()
        {

            T t = null;

            StringReader sr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(sr);

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));

            t = xs.Deserialize(xr) as T;

            return t;
        }

        public static string ToXml(object o)
        {
            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(o.GetType());

                xs.Serialize(sw, o);
            }

            return sb.ToString();
        }

        public static string FromFile(string file, Encoding encoding, bool throwExceptionIfNotFound)
        {
            if (!File.Exists(file))
            {
                if (throwExceptionIfNotFound)
                {
                    throw new FileNotFoundException(file);
                }
                else
                {
                    return null;
                }
            }

            if (encoding == null)
                encoding = Encoding.Default;

            return File.ReadAllText(file, encoding);
        }

        public static T FromFile<T>(string file, Encoding encoding, SerializeType stype, bool throwExceptionIfNotFound)
            where T : class,new()
        {
            string content = FromFile(file, encoding, throwExceptionIfNotFound);

            if (stype == SerializeType.Json)
            {
                return FromJson<T>(content);
            }
            else if (stype == SerializeType.Xml)
            {
                return FromXml<T>(content);
            }
            else
            {
                throw new NotSupportedException(stype.ToString());
            }
        }

        public static void ToFile(string file, string content, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, encoding))
                {
                    sw.Write(content);
                    sw.Flush();
                    fs.Flush();
                }
            }
        }

        public static void ToFile(string file, object o, SerializeType stype, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.Default;


            string content = string.Empty;

            if (stype == SerializeType.Json)
            {
                content = ToJson(o);
            }
            else if (stype == SerializeType.Xml)
            {
                content = ToXml(o);
            }
            else
            {
                throw new NotSupportedException(stype.ToString());
            }

            ToFile(file, content, encoding);


        }

        public static string ToJson(object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        public static T FromJson<T>(string json)
            where T : class,new()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }


        public static List<string> ReadAllLines(string file, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            if (!File.Exists(file))
            {
                return null;
            }

            string[] contents = File.ReadAllLines(file, encoding);

            return new List<string>(contents);
        }
    }
}

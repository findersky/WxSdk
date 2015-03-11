using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    public static class JsonService
    {
        public static T FromJson<T>(string json)
                        where T : class,new()
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static T FromStream<T>(Stream stream, Encoding encoding)
                        where T : class,new()
        {
            string json = string.Empty;
            using (StreamReader sr = new StreamReader(stream, encoding))
            {
                json = sr.ReadToEnd();
            }

            return FromJson<T>(json);
        }

        public static object FromStream(Type t, Stream stream, Encoding encoding)
        {
            string json = string.Empty;
            using (StreamReader sr = new StreamReader(stream, encoding))
            {
                json = sr.ReadToEnd();
            }

            return FromJson(t, json);
        }

        public static object FromJson(Type t, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, t);
        }


        public static T FromFile<T>(string file, Encoding encoding, bool catchFileNotFound)
                        where T : class,new()
        {
            if (!File.Exists(file))
            {
                if (catchFileNotFound)
                    return null;
                else
                {
                    throw new FileNotFoundException(file);
                }
            }

            string json = File.ReadAllText(file, encoding);

            return FromJson<T>(json);
        }


        public static string ToJson(object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }
    }
}

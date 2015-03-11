using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Pioneer.WxSdk.Message
{
    public static class MessageSerializer
    {
        static MessageSerializer()
        {
            RequestMessageTypes.Add(MessageType.Text, typeof(TextRequest));
            RequestMessageTypes.Add(MessageType.Image, typeof(ImageRequest));
            RequestMessageTypes.Add(MessageType.Voice, typeof(VoiceRequest));
            RequestMessageTypes.Add(MessageType.Video, typeof(VideoRequest));
            RequestMessageTypes.Add(MessageType.Location, typeof(LocationRequest));
            RequestMessageTypes.Add(MessageType.Link, typeof(LinkRequest));


            RequestEventTypes.Add(EventType.Subscribe, typeof(SubscribeEvent));
            RequestEventTypes.Add(EventType.Unsubscribe, typeof(EventRequest));
            RequestEventTypes.Add(EventType.Scan, typeof(SubscribeEvent));
            RequestEventTypes.Add(EventType.Location, typeof(LocationEvent));
            RequestEventTypes.Add(EventType.click, typeof(EventRequest));
            RequestEventTypes.Add(EventType.View, typeof(EventRequest));
            RequestEventTypes.Add(EventType.MASSSENDJOBFINISH, typeof(MassSendFinishEvent));
            RequestEventTypes.Add(EventType.Enter, typeof(EventRequest));
        }





        static ILog logger = LogFactory.GetLogger<MessageCenter>();

        public static RequestMessage DeSerialize(Stream stream, string encrypttype, PublicAccount pi)
        {
            XmlDocument x = new XmlDocument();

            x.Load(stream);

            logger.Trace("sourceXml");
            logger.Trace(x.OuterXml);

            if (encrypttype == "raw")
            {//明文模式  直接反序列化
                return DeSerialize(x.OuterXml);
            }
            else if (encrypttype == "aes")
            {
                using (StringReader sr = new StringReader(x.OuterXml))
                {
                    using (XmlReader xr = XmlReader.Create(sr))
                    {

                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(RequestMessage));

                        RequestMessage rm = xs.Deserialize(xr) as RequestMessage;

                        MessageCryptor mc = new MessageCryptor(pi.MessageToken, pi.EncryptionKey, pi.AppId);
                        string emessage = mc.Decrypt(rm.Encrypt);
                        return DeSerialize(emessage);
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        public static RequestMessage DeSerialize(string xml)
        {
            XmlDocument x = new XmlDocument();

            x.LoadXml(xml);

            string messageType = x.SelectSingleNode("/xml/MsgType").InnerText.ToLower();
            Type t;

            if (messageType == "event")
            {
                string etype = x.SelectSingleNode("/xml/Event").InnerText.ToLower();
                t = RequestEventTypes[etype];
            }
            else
            {
                t = RequestMessageTypes[messageType.ToLower()];
            }

            if (t == null)
            {
                logger.Error(x.OuterXml);
                return null;
            }

            using (StringReader sr = new StringReader(x.OuterXml))
            {
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(t);
                    RequestMessage rm = xs.Deserialize(xr) as RequestMessage;
                    rm.SourceXml = x.OuterXml;
                    return rm;
                }
            }
        }

        static Dictionary<string, Type> RequestMessageTypes = new Dictionary<string, Type>();

        static Dictionary<string, Type> RequestEventTypes = new Dictionary<string, Type>();




    }
}

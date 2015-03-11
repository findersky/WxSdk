using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pioneer.WxSdk.Message
{
    [XmlRoot("xml")]
    public class SubscribeEvent : EventRequest
    {
        [XmlElement("Ticket")]
        public string Ticket { get; set; }

        [XmlIgnore]
        public int? SceneId
        {
            get
            {
                int sceneId = 0;
                string eventKey = this.EventKey;

                string scenceString = null;

                if (this.Event.ToLower() == EventType.Subscribe)
                {
                    if (!string.IsNullOrWhiteSpace(eventKey) && eventKey.IndexOf('_') > 0)
                    {
                        string[] param = eventKey.Split('_');
                        scenceString = param[1];
                    }
                }
                else if (this.Event.ToLower() == EventType.Scan)
                {
                    scenceString = eventKey;
                }
                else
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(scenceString))
                {
                    if (int.TryParse(scenceString, out sceneId))
                    {
                        return sceneId;
                    }
                }

                return null;
            }
        }
    }
}

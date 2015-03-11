using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Message
{
    /// <summary>
    /// 默认监听器
    /// </summary>
    public class DefaultMessageListener : IMessageListener
    {

        public virtual ResponseMessage ProcessRequest(RequestMessage request)
        {
            switch (request.MsgType)
            {
                case MessageType.Text:
                    return TextRequest(request as TextRequest);

                case MessageType.Image:
                    return ImageRequest(request as ImageRequest);

                case MessageType.Voice:
                    return VoiceRequest(request as VoiceRequest);

                case MessageType.Video:
                    return VideoRequest(request as VideoRequest);

                case MessageType.Link:
                    return LinkRequest(request as LinkRequest);

                case MessageType.Location:
                    return LocationRequest(request as LocationRequest);

                case MessageType.Event:
                    {
                        EventRequest er = request as EventRequest;

                        switch (er.Event.ToLower())
                        {
                            case EventType.Subscribe:
                                return SubscribeEventRequest(er as SubscribeEvent);

                            case EventType.Unsubscribe:
                                return this.UnsubscribeEventRequest(er);

                            case EventType.Enter:
                                return EnterEventRequest(er);

                            case EventType.MASSSENDJOBFINISH:
                                return MassSendFinishEventRequest(er as MassSendFinishEvent);

                            case EventType.click:
                                return ClickEventReqeust(er);

                            case EventType.View:
                                return ViewEventRequest(er);

                            case EventType.Scan:
                                return ScanEventRequest(er as SubscribeEvent);
                            case EventType.Location:
                                return LocationEventRequest(er as LocationEvent);

                            default:
                                throw new WxException("Not Support EventKey");
                        }
                    }
                default:
                    throw new WxException("NotSupport MessageType");
            }

        }

        public virtual void Dispose()
        {
        }

        protected virtual ResponseMessage TextRequest(TextRequest textRequest)
        {
            
            TextResponse tr = new TextResponse(textRequest);

            tr.Content = textRequest.Content;

            tr.Content += "<a href='http://www.qanso.com'>首页</a>";

            return tr;
        }

        ResponseMessage DefaultText(RequestMessage request)
        {
            TextResponse tr = new TextResponse(request);

            tr.Content = XmlService.ToXml(request);

            return tr;
        }



        protected virtual ResponseMessage ImageRequest(ImageRequest imgRequest)
        {
            NewsResponse nr = new NewsResponse(imgRequest);

            nr.Articles.Add(new Article()
            {

                Title = "Title",
                Description = "Description:来源消息",
                PicUrl = imgRequest.PicUrl
            });

            return nr;
        }

        protected virtual ResponseMessage VoiceRequest(VoiceRequest voiceRequest)
        {
            return DefaultText(voiceRequest);
        }

        protected virtual ResponseMessage VideoRequest(VideoRequest videoRequest)
        {
            return DefaultText(videoRequest);
        }

        protected virtual ResponseMessage LinkRequest(LinkRequest linkRequest)
        {
            return DefaultText(linkRequest);
        }

        protected virtual ResponseMessage LocationRequest(LocationRequest locationRequest)
        {
            return DefaultText(locationRequest);
        }



        protected virtual ResponseMessage EnterEventRequest(EventRequest evRequest)
        {
            return DefaultText(evRequest);
        }

        protected virtual ResponseMessage SubscribeEventRequest(SubscribeEvent subscribeEvent)
        {
            return DefaultText(subscribeEvent);
        }

        protected virtual ResponseMessage ScanEventRequest(SubscribeEvent subscribeEvent)
        {
            return DefaultText(subscribeEvent);
        }

        protected virtual ResponseMessage MassSendFinishEventRequest(MassSendFinishEvent msfRequest)
        {
            return DefaultText(msfRequest);
        }

        protected virtual ResponseMessage ViewEventRequest(EventRequest evRequest)
        {
            return DefaultText(evRequest);
        }

        protected virtual ResponseMessage ClickEventReqeust(EventRequest evRequest)
        {
            return DefaultText(evRequest);
        }


        protected virtual ResponseMessage LocationEventRequest(LocationEvent leRequest)
        {
            return DefaultText(leRequest);
        }

        protected virtual ResponseMessage UnsubscribeEventRequest(EventRequest evRequest)
        {
            return DefaultText(evRequest);
        }
    }
}

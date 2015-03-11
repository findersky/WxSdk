using System;

namespace Pioneer.WxSdk.Message
{
    public abstract class ResponseMessage
    {
        protected ILog logger;

        ResponseMessage()
        {
            this.CreateTime = DateTime.Now;
            logger = LogFactory.GetLogger(this.GetType());
        }

        protected ResponseMessage(string messsageType)
            : this()
        {
            this._messageType = messsageType;
        }

        protected ResponseMessage(string messageType, RequestMessage rm)
            : this(messageType)
        {
            this.ToUserName = rm.FromUserName;
            this.FromUserName = rm.ToUserName;
        }

        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; }

        string _messageType;
        public string MessageType
        {
            get
            {
                return _messageType;
            }
        }

        public virtual Tuple<bool, string> Validate()
        {
            if (string.IsNullOrEmpty(this.MessageType))
            {
                return Tuple.Create(false, "MessageType is null");
            }

            if (string.IsNullOrEmpty(this.FromUserName))
            {
                return Tuple.Create(false, "FromUserName is null");
            }

            if (string.IsNullOrEmpty(this.ToUserName))
            {
                return Tuple.Create(false, "ToUserName is null");
            }

            return this.DoValidate();
        }

        protected Tuple<bool, string> DoValidate()
        {
            return Tuple.Create(true, string.Empty);
        }

        public virtual string ToXml()
        {
            Tuple<bool, string> validateResult = this.Validate();

            if (!validateResult.Item1)
                throw new WxException(validateResult.Item2);

            string template = "<xml>" + this.InnerToxml() + "</xml>";

            return template;
        }

        protected virtual string InnerToxml()
        {
            string tmp = @"<ToUserName>{0}</ToUserName>
<FromUserName>{1}</FromUserName>
<CreateTime>{2}</CreateTime>
<MsgType>{3}</MsgType>";


            return string.Format(tmp, this.ToUserName, this.FromUserName, SdkHelper.ToUnixTime(this.CreateTime), this.MessageType);
        }
    }
}

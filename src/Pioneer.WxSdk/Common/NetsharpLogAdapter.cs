using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if Pioneer

namespace Pioneer.WxSdk
{
    using Netsharp.Core;

    /// <summary>
    /// Netsharp Log 适配器
    /// </summary>
    public class NetsharpLogAdapter : ILog
    {
        INLogger innerLog;

        public NetsharpLogAdapter(string name)
        {
            innerLog = Netsharp.Core.LogFactory.GetLogger(name);
        }

        public NetsharpLogAdapter(Type type)
        {
            innerLog = Netsharp.Core.LogFactory.GetLogger(type);
        }


        public void Debug(string message)
        {
            innerLog.Debug(message);
        }

        public void Error(Exception e)
        {
            innerLog.Error(e);
        }

        public void Error(string message)
        {
            innerLog.Error(message);
        }

        public void Trace(string message)
        {
            innerLog.Trace(message);
        }

        public void Info(string message)
        {
            innerLog.Info(message);
        }

        public void Warning(string message)
        {
            innerLog.Warn(message);
        }

        public void Error(string message, Exception e)
        {
            innerLog.Error(e, message);
        }
    }
}
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    public interface ILog
    {
        void Debug(string message);

        void Error(Exception e);

        void Error(string message);

        void Trace(string message);

        void Info(string message);

        void Warning(string message);

        void Error(string message, Exception e);
    }

    public class LogFactory
    {

        internal static ILog GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        internal static ILog GetLogger(Type t)
        {
            if (SdkSetup.LogProvider == null)
                return SdkLogger.Instance;
            return SdkSetup.LogProvider(t.FullName);
        }

        internal static ILog GetLogger(string name)
        {
            if (SdkSetup.LogProvider == null)
                return SdkLogger.Instance;
            return SdkSetup.LogProvider(name);
        }
    }



    class SdkTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            string WriteMessage = DateTime.Now.ToLocalTime() + " :" + message + Environment.NewLine;

            File.AppendAllText("log.txt", WriteMessage);
        }

        public override void WriteLine(string message)
        {
            this.Write(message);
        }
    }

    public class SdkLogger : ILog
    {
        static SdkLogger _log;
        public static SdkLogger Instance
        {
            get
            {
                if (_log == null)
                    _log = new SdkLogger();
                return _log;
            }
        }

        SdkLogger()
        {
            System.Diagnostics.Debug.Listeners.Add(new SdkTraceListener());
        }

        public void Debug(string message)
        {

            System.Diagnostics.Debug.Write(message);
        }

        public void Error(Exception e)
        {
            Exception be = e.GetBaseException();

            System.Diagnostics.Debug.Write(be.Message);
            System.Diagnostics.Debug.Write(be.StackTrace);
        }

        public void Error(string message)
        {

            System.Diagnostics.Debug.Write(message);
        }

        public void Trace(string message)
        {

            System.Diagnostics.Debug.Write(message);
        }

        public void Info(string message)
        {
            System.Diagnostics.Debug.Write(message);
        }


        public void Warning(string message)
        {

            System.Diagnostics.Debug.Write(message);
        }

        public void Error(string message, Exception e)
        {

            System.Diagnostics.Debug.Write(message);

            if (e != null)
            {
                Exception baseException = e.GetBaseException();
                System.Diagnostics.Debug.Write(e.Message);
                System.Diagnostics.Debug.Write(e.StackTrace);
            }
        }
    }

}

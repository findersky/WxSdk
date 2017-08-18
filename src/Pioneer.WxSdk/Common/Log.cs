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
                return LogFactory.Instance;
            return SdkSetup.LogProvider(t.FullName);
        }

        internal static ILog GetLogger(string name)
        {
            if (SdkSetup.LogProvider == null)
                return LogFactory.Instance;
            return SdkSetup.LogProvider(name);
        }

        static ConsoleLogger _log;
        public static ConsoleLogger Instance
        {
            get
            {
                if (_log == null)
                    _log = new ConsoleLogger();
                return _log;
            }
        }
    }



    public class ConsoleLogger : ILog
    {


        public void Debug(string message)
        {

            Console.WriteLine(message);
        }

        public void Error(Exception e)
        {
            Exception be = e.GetBaseException();

            Console.WriteLine(be.Message);
            Console.WriteLine(be.StackTrace);
        }

        public void Error(string message)
        {

            Console.WriteLine(message);
        }

        public void Trace(string message)
        {

            Console.WriteLine(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }


        public void Warning(string message)
        {

            Console.WriteLine(message);
        }

        public void Error(string message, Exception e)
        {

            Console.WriteLine(message);

            if (e != null)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }

}

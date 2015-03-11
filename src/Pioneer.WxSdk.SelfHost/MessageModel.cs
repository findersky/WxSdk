using Netsharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.WxSdk.SelfHost
{
    public class MessageModel
    {
        public Task ProcessRequest(Microsoft.Owin.IOwinContext context, Func<Task> nextTask)
        {
            string path = context.Request.Uri.AbsolutePath;

            if (path != "/msg")
            {
                return nextTask();
            }

            if (context.Request.Method == "POST")
            {
                context.Response.Write("    ");
            }

            return Message.MessageCenter.ProcessRequestAsync(context.Request.Uri.OriginalString, context.Request.Method, context.Request.Body, (s, e) =>
              {
                  if (e != null)
                  {
                      INLogger logger = Netsharp.Core.LogFactory.GetLogger(this.GetType());

                      logger.Error(e);
                      return;
                  }

                  context.Response.WriteAsync(s);
              });

        }
    }
}

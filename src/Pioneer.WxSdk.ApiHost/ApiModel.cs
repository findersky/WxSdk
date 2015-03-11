using Pioneer.WxSdk.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Pioneer.WxSdk.ApiHost
{
    public class ApiModel
    {
        internal static System.Collections.Concurrent.ConcurrentDictionary<string, Type> apis;

        public static void Init()
        {
            apis = new System.Collections.Concurrent.ConcurrentDictionary<string, Type>();

            var ts = typeof(SdkSetup).Assembly.GetTypes();

            foreach (var t in ts)
            {
                var at = t.GetCustomAttribute<ApipathAttribute>(false);

                if (at == null)
                    continue;
                apis.TryAdd(at.Path, t);
            }
        }

        Func<IDictionary<string, object>, Task> _next;
        public ApiModel(Func<IDictionary<string, object>, Task> next)
        {
            this._next = next;
        }


        public Task Invoke(IDictionary<string, object> env)
        {
            Microsoft.Owin.OwinContext ctx = new Microsoft.Owin.OwinContext(env);

            string path = ctx.Request.Uri.AbsolutePath;

            Type t = null;

            if (!apis.TryGetValue(path, out t))
            {
                return _next(env);
            }

            IRequest req = Pioneer.WxSdk.JsonService.FromStream(t, ctx.Request.Body, Encoding.UTF8) as IRequest;

            if (req == null)
            {
                ctx.Response.StatusCode = 400;
                return ctx.Response.WriteAsync("");
            }

            if (SdkConfig.Instance.Deploy != Deploy.Server)
            {
                SdkConfig.Instance.Deploy = Deploy.Server;
            }

            dynamic rsp = null;

            try
            {
                rsp = req.GetResponse();
                rsp.Successed = true;
            }
            catch (WxException we)
            {
                rsp = new System.Dynamic.ExpandoObject();
                rsp.Successed = false;
                rsp.ErrorMessage = we.Message;
                rsp.ErrorCode = we.ErrorCode;
            }
            catch (Exception e)
            {
                rsp = new System.Dynamic.ExpandoObject();
                rsp.Successed = false;
                rsp.ErrorMessage = e.Message;
                rsp.ErrorCode = "";
            }

            string retjson = JsonService.ToJson(rsp);

            ctx.Response.ContentType = "text/json,charset=UTF8";

            return ctx.Response.WriteAsync(retjson);
        }
    }
}

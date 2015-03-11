using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.WxSdk.ApiHost
{
    class ApiInfoModel
    {
        Func<IDictionary<string, object>, Task> _next;
        public ApiInfoModel(Func<IDictionary<string, object>, Task> next)
        {
            this._next = next;
        }


        public Task Invoke(IDictionary<string, object> env)
        {
            Microsoft.Owin.OwinContext ctx = new Microsoft.Owin.OwinContext(env);

            if (ctx.Request.Uri.AbsolutePath != "/apiinfo")
            {
                return _next(env);
            }

            string retHtml = string.Join("</br>", ApiModel.apis.Keys.ToArray());

            ctx.Response.ContentType = "text/html,charset=UTF8";

            return ctx.Response.WriteAsync(retHtml);

        }
    }
}

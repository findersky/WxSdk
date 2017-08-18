

using Microsoft.AspNetCore.Http;
using Pioneer.WxSdk.Message;
using System.Threading.Tasks;

namespace Pioneer.WxSdk.Service
{

    public interface IWxService
    {
        Task Invoke(HttpContext context);
    }

    public class WxService : IWxService
    {


        public async Task Invoke(HttpContext context)
        {
            var url = context.Request.QueryString.Value;
            var method = context.Request.Method;
            var stream = context.Request.Body;

            await MessageCenter.ProcessRequestAsync(url, method, stream, (s, e) =>
            {
                if (e != null)
                {
                    return;
                }


                context.Response.WriteAsync(s);
            });
        }


    }


}
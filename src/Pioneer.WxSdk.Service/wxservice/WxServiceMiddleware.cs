using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Pioneer.WxSdk.Service
{
    /// <summary>
    /// Enables directory browsing
    /// </summary>
    public class EAPServiceMiddleware
    {
        ILogger logger;
        private readonly RequestDelegate _next;

        IWxService service;

        /// <summary>
        /// Creates a new instance of the SendFileMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="hostingEnv">The <see cref="IHostingEnvironment"/> used by this middleware.</param>
        /// <param name="encoder">The <see cref="HtmlEncoder"/> used by the default <see cref="HtmlDirectoryFormatter"/>.</param>
        /// <param name="options">The configuration for this middleware.</param>
        public EAPServiceMiddleware(RequestDelegate next, IWxService service, ILoggerFactory factory)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            //if (hostingEnv == null)
            //{
            //    throw new ArgumentNullException(nameof(hostingEnv));
            //}

            //if (options == null)
            //{
            //    throw new ArgumentNullException(nameof(options));
            //}

            logger = factory.CreateLogger<WxService>();

            //_options = options.Value;
            _next = next;
            this.service = service;
        }

        /// <summary>
        /// Examines the request to see if it matches a configured directory.  If so, a view of the directory contents is returned.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/wx")
            {
               await this.service.Invoke(context);
            }
            await _next(context);
        }
    }
}
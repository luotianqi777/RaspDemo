using Microsoft.AspNetCore.Http;
using System.Text;

namespace AgentDemo
{
    public static partial class XTool
    {
        public class HttpHelper
        {

            /// <summary>
            /// 判断一个请求是否需要转发(通过XMFLOW标识)
            /// </summary>
            /// <param name="request">要判断的请求</param>
            /// <returns>是返回true，否则返回false</returns>
            public static bool IsNeedRequest(HttpRequest request)
            {
                return request.Headers.TryGetValue("XMFLOW", out _);
            }

            /// <summary>
            /// 判断一个请求是否需要检测(通过XMIAST标识)
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            public static bool IsNeedCheck(HttpRequest request)
            {
                return request.Headers.TryGetValue("XMIAST", out _);
            }

            public static HttpContext GetCurrentHttpContext()
            {
                return new HttpContextAccessor().HttpContext;
            }

            public static HttpRequest GetCurrentHttpRequest()
            {
                return GetCurrentHttpContext().Request;
            }

            public static string GetCurrentUrl()
            {
                return GetUrl(GetCurrentHttpRequest());
            }

            /// <summary>
            /// 获取请求的完整url
            /// </summary>
            /// <param name="request">request请求</param>
            /// <returns>请求的url</returns>
            public static string GetUrl(HttpRequest request)
            {
                return new StringBuilder()
                    .Append(request.Scheme)
                    .Append("://")
                    .Append(request.Host)
                    .Append(request.PathBase)
                    .Append(request.Path)
                    .Append(request.QueryString)
                    .ToString();
            }
        }
    }
}

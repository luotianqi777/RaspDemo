using Microsoft.AspNetCore.Http;
using System.Text;

namespace AgentDemo
{
    public static partial class XTool
    {
        public class HttpHelper
        {

            /// <summary>
            /// 判断一个请求是否为IAST检测请求
            /// </summary>
            /// <param name="request">要判断的请求</param>
            /// <returns>是返回true，否则返回false</returns>
            public static bool IsCheckRequest(HttpRequest request)
            {
                return request.Headers.TryGetValue("XMFLOW", out _);
            }

            public static HttpContext GetCurrentHttpContext()
            {
                return new HttpContextAccessor().HttpContext;
            }

            public static string GetCurrentUrl()
            {
                return GetUrl(GetCurrentHttpContext().Request);
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

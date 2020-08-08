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
                if (request == null) { return false; }
                return !request.Headers.TryGetValue("XMFLOW", out _);
            }

            /// <summary>
            /// 判断一个请求是否需要检测(通过XMIAST标识)
            /// </summary>
            /// <param name="request"></param>
            /// <returns>是返回true，否则返回false</returns>
            public static bool IsNeedCheck(HttpRequest request)
            {
                if (request == null) { return false; }
                return request.Headers.TryGetValue("XMIAST", out _);
            }

            public static HttpContext GetCurrentHttpContext()
            {
                return new HttpContextAccessor().HttpContext;
            }

            public static HttpRequest GetCurrentHttpRequest()
            {
                return GetCurrentHttpContext()?.Request;
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
                if (request != null)
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
                else
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// 获取请求数据，若非Post返回空字符串
            /// </summary>
            /// <param name="request">请求</param>
            /// <returns>请求的数据</returns>
            public static string GetRequestBody(HttpRequest request)
            {
                if (!request.HasFormContentType)
                {
                    // 非Post返回空字符串
                    return string.Empty;
                }
                StringBuilder body = new StringBuilder();
                // 获取boundary
                var boundary = request.ContentType.Substring(request.ContentType.IndexOf("boundary") + 9);
                foreach (var file in request.Form.Files)
                {
                    body.Append($"{boundary}\r\n");
                    body.Append($"Content-Disposition: form-data; name=\"{file.Name}\"; filename=\"{file.FileName}\"\r\n");
                    body.Append($"Content-Type: {file.ContentType}\r\n\r\n");
                    var steam = file.OpenReadStream();
                    var data = new byte[steam.Length];
                    steam.Read(data);
                    body.Append(Encoding.UTF8.GetString(data));
                }
                body.Append($"\r\n{boundary}--");
                return body.ToString();
            }

        }
    }
}

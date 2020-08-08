using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class Request : Msg
        {
            #region Property
            [JsonProperty("result")]
            public XResult Result { get; set; }
            [JsonProperty("cmd")]
            public int Cmd { get; set; }

            public class XResult : JsonBase
            {
                [JsonProperty("urls")]
                public XUrls[] Urls { get; set; }
                public class XUrls : JsonBase
                {
                    [JsonProperty("method")]
                    public string Method { get; set; }
                    [JsonProperty("url")]
                    public string Url { get; set; }
                    [JsonProperty("data")]
                    public string Data { get; set; }
                    [JsonProperty("headers")]
                    public XHeaders Headers { get; set; }
                    [JsonProperty("iastrange")]
                    public string[] Iastrange { get; set; }
                    public class XHeaders : JsonBase
                    {
                        [JsonProperty("Cookie")]
                        public string Cookie { get; set; }
                        [JsonProperty("Accept")]
                        public string Accept { get; set; }
                        [JsonProperty("Upgrade-Insecure-Requests")]
                        public string Upgrade { get; set; }
                        [JsonProperty("Connection")]
                        public string Connection { get; set; }
                        [JsonProperty("Referer")]
                        public string Referer { get; set; }
                        [JsonProperty("User-Agent")]
                        public string UserAgent { get; set; }
                        [JsonProperty("Host")]
                        public string Host { get; set; }
                        [JsonProperty("Accept-Encoding")]
                        public string AcceptEncoding { get; set; }
                        [JsonProperty("Accept-Language")]
                        public string AcceptLanguage { get; set; }
                    }
                }
            }
            #endregion

            /// <summary>
            /// 获取一个请求转发Json数据
            /// </summary>
            /// <param name="request">要转发的请求</param>
            /// <param name="iastrange">漏洞检测范围</param>
            public static Request GetInstance(HttpRequest request, params string[] iastrange)
            {
                if (request == null) { return null; }
                var headers = request.Headers;
                var url =  XTool.HttpHelper.GetUrl(request);
                // 找不到直接获取param的办法，目前通过搜索?来截取
                var index = url.IndexOf('?');
                if (index == -1 && request.Method == "GET" && iastrange.Length != 0)
                {
                    throw new Exception("需要转发的Get检测请求中找不到param");
                }
                var referer = index == -1 ? url : url.Substring(0, index);
                var data = GetRequestBody(request);
                return new Request
                {
                    Cmd = 4001,
                    Result = new XResult
                    {
                        Urls = new XResult.XUrls[] {
                            new XResult.XUrls {
                                Method = request.Method,
                                Url = url,
                                Data = data,
                                Headers = new XResult.XUrls.XHeaders
                                {
                                    Cookie = headers["Cookie"],
                                    Accept = headers["Accept"],
                                    Upgrade = headers["Upgrade-Insecure-Requests"],
                                    Connection = headers["Connection"],
                                    Referer = referer,
                                    UserAgent = headers["User-Agent"],
                                    Host = headers["Host"],
                                    AcceptEncoding = headers["Accept-Encoding"],
                                    AcceptLanguage = headers["Accept-Language"],
                                },
                                Iastrange = iastrange
                            }
                        },
                    }
                };
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

﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System.Net;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class Request : JsonData.XMsg
        {
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

            /// <summary>
            /// 获取一个请求转发Json数据
            /// </summary>
            /// <param name="request">要转发的请求</param>
            /// <param name="iastrange">漏洞检测范围</param>
            public Request(HttpRequest request, params string[] iastrange)
            {
                if (iastrange.Length == 0)
                {
                    Debuger.WriteLine("警告：iastrange的个数为零");
                }
                var headers = request.Headers;

                Cmd = 4001;
                Result = new XResult
                {
                    Urls = new XResult.XUrls[] {
                    new XResult.XUrls {
                        Method = request.Method,
                        Url=XTool.HttpHelper.GetUrl(request),
                        Data = "",
                        Headers = new XResult.XUrls.XHeaders
                        {
                            Cookie=headers["Cookie"],
                            Accept=headers["Accept"],
                            Upgrade=headers["Upgrade-Insecure-Requests"],
                            Connection=headers["Connection"],
                            Referer=headers["Referer"],
                            UserAgent=headers["User-Agent"],
                            Host=headers["Host"],
                            AcceptEncoding=headers["Accept-Encoding"],
                            AcceptLanguage=headers["Accept-Language"]
                        },
                        Iastrange = iastrange
                    }
                }
                };
            }
        }

    }
}
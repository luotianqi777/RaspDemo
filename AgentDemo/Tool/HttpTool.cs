using AgentDemo.Json;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AgentDemo
{
    public partial class Tool
    {
        public partial class Http
        {

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

            public static async void RequestForward(string targetIp, string agentID,string key,string nonce, HttpRequest request)
            {
                var datajson = XJsonData.GetXJson(agentID, XRequest.GetInstance(request));
                var encodeJson = Json.AESEncrypt(datajson.ToString(), key,nonce);
                Debuger.WriteLine(datajson);
                var httpContent = new StringContent(encodeJson, Encoding.UTF8, "application/json");
                using var httpClient = new HttpClient();
                var httpResponse = await httpClient.PostAsync(targetIp, httpContent);
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                }
            }


        }
    }
}

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

            /// <summary>
            /// 请求转发
            /// </summary>
            /// <param name="requestUrl">发送目标的url</param>
            /// <param name="agentID">插件ID</param>
            /// <param name="aesTag">aes加密Tag</param>
            /// <param name="aesNonce">aes加密nonce</param>
            /// <param name="request">要转发的request</param>
            public static async void RequestForward(string requestUrl, string agentID,string aesTag,string aesNonce, HttpRequest request)
            {
                var datajson = XJsonData.GetXJson(agentID, XRequest.GetInstance(request, "sql"));
                var encodeJson = Json.AESEncrypt(datajson.ToString(), aesTag,aesNonce);
                XAesResult result = new XAesResult
                {
                    Id = agentID,
                    Aes = encodeJson,
                    AesTag = Json.StrToBase64(aesTag),
                    AesNonce = Json.StrToBase64(aesNonce)
                };
                Debuger.WriteLine("信息已发送");
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/json");
                using var httpClient = new HttpClient();
                var httpResponse = await httpClient.PostAsync(requestUrl, httpContent);
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Debuger.WriteLine(responseContent);
                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                }
            }


        }
    }
}

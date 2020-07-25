using AgentDemo.Json;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AgentDemo
{
    public partial class Tool
    {
        public partial class Http
        {

            #region GetUrl
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
            #endregion

            #region RequestForward
            /// <summary>
            /// 请求转发
            /// </summary>
            /// <param name="srcRequest">要转发的request</param>
            /// <param name="targetUrl">发送目标的url</param>
            /// <param name="agentID">插件ID</param>
            /// <param name="aesTag">aes加密Tag</param>
            /// <param name="aesNonce">aes加密nonce</param>
            public static void RequestForwardAsync(HttpRequest srcRequest, string ip, int port, string agentID, string aesTag, string aesNonce)
            {
                var datajson = XJsonData.GetXJsonData(agentID, XRequest.GetInstance(srcRequest, "sql"));
                var encodeJson = Json.AESEncrypt(datajson.ToString(), aesTag, aesNonce);
                XAesResult result = new XAesResult
                {
                    Id = agentID,
                    Aes = encodeJson,
                    AesTag = Json.StrToBase64(aesTag),
                    AesNonce = Json.StrToBase64(aesNonce)
                };
                // 请求转发
                var response = SendRequestAsync(ip, port, result.ToString());
                Debuger.WriteLine(response);
            }
            #endregion

            #region SendRequest
            // public static async Task<string> SendRequestAsync(string url, string message)
            public static string SendRequestAsync(string ip, int port, string jsonMessage)
            {
                #region Http
                // string url = $"http://{ip}:{port}";

                // // HttpClient client = new HttpClient();
                // // var response = client.PostAsync(url, new StringContent(message, Encoding.UTF8, "application/json"));
                // // return await response.Result.Content.ReadAsStringAsync();

                // HttpWebRequest request = WebRequest.CreateHttp(url);
                // request.Method = "POST";
                // request.ContentType = "application/json";
                // request.Timeout = 5 * 1000;
                // using (var writeStream = new StreamWriter(request.GetRequestStream()))
                // {
                //     writeStream.Write(jsonMessage);
                //     writeStream.Close();
                //     try
                //     {
                //         var response = request.GetResponse();
                //         using (var readStream = new StreamReader(response.GetResponseStream()))
                //         {
                //             var responseString = readStream.ReadToEnd();
                //             readStream.Close();
                //             return responseString;
                //         }
                //     }
                //     catch(Exception e)
                //     {
                //         Debuger.WriteLine(e.Message + e.Source + e.StackTrace);
                //         return "";
                //     }
                // }
                #endregion

                #region Socket
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                    Debuger.WriteLine("连接成功");
                }
                catch
                {
                    Debuger.WriteLine("连接失败");
                    return "";
                }
                // 发送
                byte[] sendData = Encoding.UTF8.GetBytes(jsonMessage);
                socket.Send(sendData);
                Debuger.WriteLine("请求已转发");
                // 接收
                while (true)
                {
                    byte[] data = new byte[1024];
                    socket.Receive(data);
                    string stringData = Encoding.UTF8.GetString(data);
                    if (!string.IsNullOrWhiteSpace(stringData))
                    {
                        return stringData;
                    }
                }
                #endregion
            }
            #endregion

        }
    }
}

using AgentDemo.Json;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AgentDemo
{
    public static partial class Tool
    {
        public class XHttpHelper
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
            /// 把请求包装成Json信息并转发
            /// </summary>
            /// <param name="agentConfig">插件配置信息</param>
            /// <param name="request">转发的请求</param>
            /// <param name="iastrange">检测标签</param>
            public static async void RequestForwardAsync(AgentConfig agentConfig,HttpRequest request,params string[] iastrange)
            {
                // 包装请求
                var datajson = XJsonData.GetXJsonData(agentConfig.AgentID, XRequest.GetInstance(request, iastrange));
                var encryptedJson = XTypeConverter.AESEncrypt(datajson.ToString(), agentConfig.AesKey, out agentConfig.AesTag, out agentConfig.AesNonce);
                XAesResult result = new XAesResult
                {
                    Id = agentConfig.AgentID,
                    Aes =encryptedJson,
                    AesTag = XTypeConverter.StrToBase64(agentConfig.AesTag),
                    AesNonce = XTypeConverter.StrToBase64(agentConfig.AesNonce)
                };
                Debuger.WriteLine(result);
                // 发送信息
                var response = await SendRequestAsync(result.ToString(), agentConfig);
                Debuger.WriteLine($"回复内容：{response}");
            }
            #endregion

            #region SendRequest
            /// <summary>
            /// 发送请求
            /// </summary>
            /// <param name="message">要发送的信息</param>
            /// <param name="agentConfig">agent配置信息</param>
            /// <returns>服务器响应内容</returns>
            public static async Task<string> SendRequestAsync(string message, AgentConfig agentConfig)
            {
                string ip = agentConfig.IP;
                int port = agentConfig.Port;
                int timeOut = agentConfig.TimeOut;

                // 创建socket
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    SendTimeout =  timeOut,
                    ReceiveTimeout = timeOut
                };

                // 连接服务器
                try
                {
                    socket.Connect(ip, port);
                    Debuger.WriteLine($"URL: {ip}:{port}");
                }
                catch (Exception e)
                {
                    Debuger.WriteLine($"连接失败，错误信息：{e.Message}");
                    return string.Empty;
                }

                // 发送请求
                string sendData = XTypeConverter.StrToBase64(message);
                byte[] sendDataByte = Encoding.UTF8.GetBytes(sendData);
                byte[] sizeByte = XTypeConverter.IntToByte(sendDataByte.Length);
                int sendSize = await socket.SendAsync(sizeByte.Concat(sendDataByte).ToArray(), SocketFlags.None);
                Debuger.WriteLine($"请求已转发，数据长度：{sendDataByte.Length}，发送长度：{sendSize}");

                // 接收响应
                byte[] dataArray = new byte[1024];
                StringBuilder response = new StringBuilder();
                while(await socket.ReceiveAsync(dataArray, SocketFlags.None)!=0)
                {
                    response.Append(Encoding.UTF8.GetString(dataArray));
                }
                Debuger.WriteLine($"回复已接收，数据长度：{response.Length}");
                return response.ToString();
            }
            #endregion

        }
    }
}

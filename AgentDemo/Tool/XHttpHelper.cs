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

            #region SendRequestAsync

            /// <summary>
            /// 将jsonData加密
            /// </summary>
            /// <param name="jsonData">要发送的json数据</param>
            /// <param name="agentConfig">Agent配置信息</param>
            /// <returns>加密后的信息</returns>
            public static string EncryptJson(XJsonData jsonData, AgentConfig agentConfig)
            {
                // Aes-Gcm加密
                var encryptedJson = XTypeConverter.AESEncrypt(jsonData.ToString(), agentConfig.AesKey, out agentConfig.AesTag, out agentConfig.AesNonce);
                // 封装成json
                XAesResult result = new XAesResult
                {
                    Id = agentConfig.AgentID,
                    Aes = encryptedJson,
                    AesTag = agentConfig.AesTag,
                    AesNonce = agentConfig.AesNonce
                };
                return result.ToString();
            }

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
                    Debuger.WriteLine(agentConfig.DEBUG,$"URL: {ip}:{port}");
                }
                catch (Exception e)
                {
                    Debuger.WriteLine(agentConfig.DEBUG,$"连接失败，错误信息：{e.Message}");
                    return string.Empty;
                }

                // 发送请求
                byte[] sendDataByte = Encoding.UTF8.GetBytes(message);
                byte[] snedDataSizeByte = XTypeConverter.IntToByte(sendDataByte.Length);
                int sendSize = await socket.SendAsync(snedDataSizeByte.Concat(sendDataByte).ToArray(), SocketFlags.None);
                Debuger.WriteLine(agentConfig.DEBUG,$"请求已发送，数据长度：{sendDataByte.Length}，发送长度：{sendSize}");

                // 接收响应
                byte[] dataArray = new byte[1024];
                StringBuilder response = new StringBuilder();
                while(await socket.ReceiveAsync(dataArray, SocketFlags.None)!=0)
                {
                    response.Append(Convert.ToBase64String(dataArray));
                }
                socket.Close();
                Debuger.WriteLine($"回复已接收，数据长度：{response.Length}");
                return Encoding.UTF8.GetString(Convert.FromBase64String(response.ToString()));
            }

            #endregion

            public static async Task<string> SendRequest(AgentConfig agentConfig, HttpRequest request, params string[] iastrange)
            {
                XJsonData jsonData = XJsonData.GetXJsonData(agentConfig.AgentID, XRequest.GetInstance(request, iastrange));
                var message = EncryptJson(jsonData, agentConfig);
                return await SendRequestAsync(message, agentConfig);
            }

            public static async Task<string> SendDoki(AgentConfig agentConfig)
            {
                XJsonData jsonData = XJsonData.GetXJsonData(agentConfig.AgentID, XDoki.GetInstance(agentConfig.LocalIP));
                var message = EncryptJson(jsonData, agentConfig);
                return await SendRequestAsync(message, agentConfig);
            }

        }
    }
}

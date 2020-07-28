using AgentDemo.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AgentDemo
{
    partial class XTool
    {
        public class JsonSender
        {
            /// <summary>
            /// 将jsonData加密
            /// </summary>
            /// <param name="jsonData">要发送的json数据</param>
            /// <param name="agentConfig">Agent配置信息</param>
            /// <returns>加密后的信息</returns>
            private static string EncryptJsonData(XJsonData jsonData, AgentConfig agentConfig)
            {
                // Aes-Gcm加密
                var encryptedJson = TypeConverter.AESEncrypt(jsonData.ToString(), agentConfig.AesKey, out agentConfig.AesTag, out agentConfig.AesNonce);
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
            /// 向服务端发送字符串
            /// </summary>
            /// <param name="message">要发送的信息</param>
            /// <param name="agentConfig">agent配置信息</param>
            /// <returns>服务器响应内容</returns>
            private static async Task<string> SendMessageAsync(string message, AgentConfig agentConfig)
            {
                string ip = agentConfig.IP;
                int port = agentConfig.Port;
                int timeOut = agentConfig.TimeOut;

                // 创建socket
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    SendTimeout = timeOut,
                    ReceiveTimeout = timeOut
                };

                // 连接服务器
                try
                {
                    socket.Connect(ip, port);
                    Debuger.WriteLine(agentConfig.DEBUG, $"Sokcet连接成功: {ip}:{port}");
                }
                catch (Exception e)
                {
                    Debuger.WriteLine(agentConfig.DEBUG, $"Socket连接失败，错误信息：{e.Message}");
                    return string.Empty;
                }

                // 发送请求
                byte[] sendDataByte = Encoding.UTF8.GetBytes(message);
                byte[] snedDataSizeByte = TypeConverter.IntToByte(sendDataByte.Length);
                int sendSize = await socket.SendAsync(snedDataSizeByte.Concat(sendDataByte).ToArray(), SocketFlags.None);
                Debuger.WriteLine(agentConfig.DEBUG, $"请求已发送，数据长度：{sendDataByte.Length}，发送长度：{sendSize}");

                // 接收响应
                byte[] dataArray = new byte[1024];
                StringBuilder response = new StringBuilder();
                while(true)
                {
                    var receiveDataSize = await socket.ReceiveAsync(dataArray, SocketFlags.None);
                    if (receiveDataSize == 0) break;
                    response.Append(Convert.ToBase64String(dataArray, 0, receiveDataSize));
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Debuger.WriteLine(agentConfig.DEBUG, $"回复已接收，数据长度：{response.Length}");
                return Encoding.UTF8.GetString(Convert.FromBase64String(response.ToString()));
            }

            /// <summary>
            /// 向服务端发送JsonData
            /// </summary>
            /// <param name="jsonData">发送的json数据</param>
            /// <param name="agentConfig">AgentID</param>
            /// <returns>服务器返回的消息</returns>
            public static async Task<string> SendJsonData(XJsonData jsonData, AgentConfig agentConfig)
            {
                Debuger.WriteLine(agentConfig.DEBUG, $"加密前数据：{jsonData}");
                var message = EncryptJsonData(jsonData, agentConfig);
                Debuger.WriteLine(agentConfig.DEBUG, $"发送的数据：{message}");
                return await SendMessageAsync(message, agentConfig);
            }

        }
    }
}

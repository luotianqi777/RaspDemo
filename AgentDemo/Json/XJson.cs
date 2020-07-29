using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static AgentDemo.XTool;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        /// <summary>
        /// 将jsonData加密
        /// </summary>
        /// <param name="jsonData">要发送的json数据</param>
        /// <param name="agentConfig">Agent配置信息</param>
        /// <returns>加密后的信息</returns>
        private static string EncryptJsonData(JsonData jsonData, AgentConfig agentConfig)
        {
            // Aes-Gcm加密
            var encryptedJson = TypeConverter.AESEncrypt(jsonData.ToString(), agentConfig.AesKey, out agentConfig.AesTag, out agentConfig.AesNonce);
            // 封装成json
            AesResult result = new AesResult
            {
                Id = agentConfig.AgentID,
                Aes = encryptedJson,
                AesTag = agentConfig.AesTag,
                AesNonce = agentConfig.AesNonce
            };
            return result.ToString();
        }

        /// <summary>
        /// 将json信息解密
        /// </summary>
        /// <param name="jsonString">要解密的字符串</param>
        /// <param name="agentConfig">插件配置</param>
        /// <returns>解密后的json字符串</returns>
        public static string DncryptJsonData(string jsonString, AgentConfig agentConfig)
        {
            AesResult result = GetJson<AesResult>(jsonString);
            return TypeConverter.AESDecrypt(result.Aes, agentConfig.AesKey, result.AesTag, result.AesNonce);
        }

        /// <summary>
        /// 向服务端发送字符串
        /// </summary>
        /// <param name="message">要发送的信息</param>
        /// <param name="agentConfig">agent配置信息</param>
        /// <returns>服务器响应内容</returns>
        private static async Task<string> SendMessageAsync(string message, AgentConfig agentConfig)
        {
            // 创建socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout = agentConfig.TimeOut,
                ReceiveTimeout = agentConfig.TimeOut
            };

            // 连接服务器
            try
            {
                IPAddress ip = IPAddress.Parse(agentConfig.IP);
                int port = agentConfig.Port;
                socket.Connect(new IPEndPoint(ip, port));
                Debuger.WriteLine(agentConfig.DEBUG, $"Sokcet连接成功: {ip}:{port}");
            }
            catch (Exception e)
            {
                Debuger.WriteLine($"Socket连接失败，错误信息：{e.Message}");
                return string.Empty;
            }

            // 发送请求
            byte[] sendDataByte = TypeConverter.StringToBytes(message);
            await socket.SendAsync(sendDataByte, SocketFlags.None);

            // 接收响应
            byte[] dataArray = new byte[1024];
            StringBuilder response = new StringBuilder();
            try
            {
                while (true)
                {
                    var receiveDataSize = await socket.ReceiveAsync(dataArray, SocketFlags.None);
                    if (receiveDataSize == 0) break;
                    response.Append(Convert.ToBase64String(dataArray, 0, receiveDataSize));
                }
            }
            catch (Exception e)
            {
                Debuger.WriteLine($"数据接收失败，错误信息：{e.Message}");
            }
            socket.Close();

            return Encoding.UTF8.GetString(Convert.FromBase64String(response.ToString()));
        }

        /// <summary>
        /// 向服务端发送JsonData
        /// </summary>
        /// <param name="jsonData">发送的json数据</param>
        /// <param name="agentConfig">AgentID</param>
        /// <returns>服务器返回的消息</returns>
        public static async Task<string> SendJsonData(JsonData jsonData, AgentConfig agentConfig)
        {
            var message = EncryptJsonData(jsonData, agentConfig);
            return await SendMessageAsync(message, agentConfig);
        }

    }
}

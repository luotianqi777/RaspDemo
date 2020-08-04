using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
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
        /// <returns>加密后的信息</returns>
        private static string EncryptJsonData(JsonData jsonData)
        {
            // Aes-Gcm加密
            var encryptedJson = TypeConverter.AESEncrypt(jsonData.ToString(), AgentConfig.AesKey, out AgentConfig.AesTag, out AgentConfig.AesNonce);
            // 封装成json
            AesResult result = new AesResult
            {
                Id = AgentConfig.AgentID,
                Aes = encryptedJson,
                AesTag = AgentConfig.AesTag,
                AesNonce = AgentConfig.AesNonce
            };
            return result.ToString();
        }

        /// <summary>
        /// 向服务端发送字符串
        /// </summary>
        /// <param name="message">要发送的信息</param>
        /// <returns>服务器响应内容</returns>
        private static async Task<string> SendMessageAsync(string message)
        {
            // 创建socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout = AgentConfig.TimeOut,
                ReceiveTimeout = AgentConfig.TimeOut
            };

            // 连接服务器
            try
            {
                IPAddress ip = IPAddress.Parse(AgentConfig.IP);
                int port = AgentConfig.Port;
                socket.Connect(new IPEndPoint(ip, port));
            }
            catch (Exception e)
            {
                Debuger.WriteLine($"Socket连接失败，错误信息：{e.Message}");
                return string.Empty;
            }

            // 发送请求
            byte[] sendDataByte = TypeConverter.StringToBytes(message);
            await socket.SendAsync(sendDataByte, SocketFlags.None);

            // 接收返回数据
            await socket.ReceiveAsync(new byte[4], SocketFlags.None);
            byte[] receiveDataByte = new byte[1024];
            StringBuilder jsonBuilder = new StringBuilder();
            while (true)
            {
                var size = await socket.ReceiveAsync(receiveDataByte, SocketFlags.None);
                if (size == 0) break;
                jsonBuilder.Append(Encoding.UTF8.GetString(receiveDataByte, 0, size));
            }
            socket.Close();
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// 向服务端发送Msg
        /// </summary>
        /// <param name="msg">发送的Msg</param>
        /// <returns>服务器响应消息解析后的JsonData</returns>
        public static async Task<string> SendJsonMsg(Msg msg)
        {
            // 封装Msg
            var jsonData = JsonData.GetInstance(msg);
            // 加密JsonData
            var message = EncryptJsonData(jsonData);
            // 发送JsonData
            var response = await SendMessageAsync(message);
            // 解析回复的AesResult
            var aesJson = JsonConvert.DeserializeObject<AesResult>(response);
            // 返回解密后的JsonData
            if (aesJson == null) return string.Empty;
            return TypeConverter.AESDecrypt(aesJson.Aes, AgentConfig.AesKey, aesJson.AesTag, aesJson.AesNonce);
        }

    }
}

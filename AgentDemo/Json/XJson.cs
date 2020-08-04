using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
        /// <returns>加密后的信息</returns>
        private static string EncryptJsonData(JsonData jsonData)
        {
            // Aes-Gcm加密
            var agentConfig = AgentConfig.GetInstance();
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
        /// 向服务端发送字符串
        /// </summary>
        /// <param name="message">要发送的信息</param>
        /// <returns>服务器响应内容</returns>
        private static async Task<string> SendMessageAsync(string message)
        {
            var agentConfig = AgentConfig.GetInstance();
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

            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(response.ToString()));
            }catch
            {
                // 编码有误
                return null;
            }
        }

        /// <summary>
        /// 向服务端发送Msg
        /// </summary>
        /// <param name="msg">发送的Msg</param>
        /// <returns>服务器响应消息</returns>
        public static async Task<string> SendJsonMsg(Msg msg)
        {
            var jsonData = JsonData.GetInstance(msg);
            var message = EncryptJsonData(jsonData);
            return await SendMessageAsync(message);
        }

        /// <summary>
        /// utf8格式的字节流转字符串并解密为JsonData，并将字节流前4bit作为字节流长度提取出来
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <param name="size">字节流长度</param>
        /// <returns>字符串</returns>
        public static string GetResponseJsonData(string utf8String, out int size)
        {
            var bytes = Encoding.UTF8.GetBytes(utf8String);
            byte[] sizeByte = new byte[4];
            Array.Copy(bytes, sizeByte, 4);
            size = TypeConverter.ByteToInt(sizeByte);
            var aesResult = JsonConvert.DeserializeObject<AesResult>(utf8String.Substring(2));
            return TypeConverter.AESDecrypt(aesResult.Aes, AgentConfig.GetInstance().AesKey, aesResult.AesTag, aesResult.AesNonce);
        }

    }
}

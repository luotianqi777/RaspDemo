using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AgentDemo
{
    public static partial class XTool
    {
        public static class TypeConverter
        {
            private readonly static int AesTagBitSize = 128;
            private readonly static int AesNonceBitSize = 128;

            /// <summary>
            /// AESGCM加密
            /// </summary>
            /// <param name="text">要加密的内容</param>
            /// <param name="key">aes密匙</param>
            /// <param name="tag">算法计算出的aestag</param>
            /// <param name="nonce">用于算法计算的随机字节组</param>
            /// <returns>加密后的内容</returns>
            public static string AESEncrypt(string text, string key, out string tag, out string nonce)
            {
                var keyByte = Encoding.UTF8.GetBytes(key);
                var textByte = Encoding.UTF8.GetBytes(text);
                var nonceByte = new byte[AesNonceBitSize/8];
                new Random().NextBytes(nonceByte);
                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(keyByte), AesTagBitSize, nonceByte);
                cipher.Init(true, parameters);
                var cipherByte = new byte[cipher.GetOutputSize(textByte.Length)];
                try
                {
                    var len = cipher.ProcessBytes(textByte, 0, textByte.Length, cipherByte, 0);
                    len += cipher.DoFinal(cipherByte, len);
                }
                catch (Exception e)
                {
                    Debuger.WriteLine(e.Message);
                }
                nonce = Convert.ToBase64String(nonceByte);
                tag = Convert.ToBase64String(cipher.GetMac());
                var result = new byte[textByte.Length];
                Array.Copy(cipherByte, result, textByte.Length);
                return Convert.ToBase64String(result);
            }

            /// <summary>
            /// AESGCM解密
            /// </summary>
            /// <param name="cipherText">加密后的内容</param>
            /// <param name="key">aes密匙</param>
            /// <param name="tag">加密算法产生的aestag</param>
            /// <param name="nonce">用于加密算法的随机字节组</param>
            /// <returns>解密后的内容</returns>
            public static string AESDecrypt(string cipherText, string key, string tag, string nonce)
            {
                var keyByte = Encoding.UTF8.GetBytes(key);
                var tagByte = Convert.FromBase64String(tag);
                var nonceByte = Convert.FromBase64String(nonce);
                var cipherByte = Convert.FromBase64String(cipherText).Concat(tagByte).ToArray();
                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(keyByte), AesTagBitSize, nonceByte);
                cipher.Init(false, parameters);
                var textByte = new byte[cipher.GetOutputSize(cipherByte.Length)];
                try
                {
                    var len = cipher.ProcessBytes(cipherByte, 0, cipherByte.Length, textByte, 0);
                    len += cipher.DoFinal(textByte, len);
                }
                catch (Exception e)
                {
                    Debuger.WriteLine(e.Message);
                }
                return Encoding.UTF8.GetString(textByte);
            }

            /// <summary>
            /// 将一个32位整数转为字节流
            /// </summary>
            /// <param name="num">要转换的整数</param>
            /// <returns>整数对应的字节流</returns>
            private static byte[] IntToByte(int num)
            {
                byte[] data = new byte[4];
                data[0] = (byte)(num & 0xFF);
                data[1] = (byte)((num & 0xFF00) >> 8);
                data[2] = (byte)((num & 0xFF0000) >> 16);
                data[3] = (byte)((num >> 24) & 0xFF);
                return data;
            }

            /// <summary>
            /// 字节流转整数
            /// </summary>
            /// <param name="bytes">要转换的字节流</param>
            /// <returns>字节流对应的整数</returns>
            private static int ByteToInt(byte[] bytes)
            {
                int value = (bytes[0] & 0xFF)
                        | ((bytes[1] & 0xFF) << 8)
                        | ((bytes[2] & 0xFF) << 16)
                        | ((bytes[3] & 0xFF) << 24);
                return value;
            }

            /// <summary>
            /// 字符串转字节流，并将字节流长度放到字节流头部
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static byte[] StringToBytes(string message)
            {
                byte[] messageByte = Encoding.UTF8.GetBytes(message);
                byte[] sizeByte = IntToByte(messageByte.Length);
                return sizeByte.Concat(messageByte).ToArray();
            }

            /// <summary>
            /// utf8格式的字节流转字符串，并将字节流前4bit作为字节流长度提取出来
            /// </summary>
            /// <param name="bytes">字节流</param>
            /// <param name="size">字节流长度</param>
            /// <returns>字符串</returns>
            public static string BytesToString(string utf8String, out int size)
            {
                var bytes = Encoding.UTF8.GetBytes(utf8String);
                byte[] sizeByte = new byte[4];
                Array.Copy(bytes, sizeByte, 4);
                size = ByteToInt(sizeByte);
                string result = utf8String.Substring(2);
                return result;
            }

        }
    }
}

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AgentDemo
{
    public static partial class XTool
    {
        public static class TypeConverter
        {
            public readonly static int AesTagLength = 16;

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
                var nonceByte = new byte[16];
                new Random().NextBytes(nonceByte);
                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(keyByte), 128, nonceByte);
                cipher.Init(true, parameters);
                var cipherByte = new byte[cipher.GetOutputSize(textByte.Length)];
                try
                {
                    var len = cipher.ProcessBytes(textByte, 0, textByte.Length, cipherByte, 0);
                    cipher.DoFinal(cipherByte, len);
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
            /// <param name="encryptedText">加密后的内容</param>
            /// <param name="key">aes密匙</param>
            /// <param name="tag">加密算法产生的aestag</param>
            /// <param name="nonce">用于加密算法的随机字节组</param>
            /// <returns>解密后的内容</returns>
            public static string AESDecrypt(string encryptedText, string key, string tag, string nonce)
            {
                byte[] tagByte = Convert.FromBase64String(tag);
                byte[] nonceByte =Convert.FromBase64String(nonce); 
                byte[] encryptedByte =Convert.FromBase64String(encryptedText);
                byte[] textByte = new byte[encryptedByte.Length];
                using (AesGcm aes = new AesGcm(Encoding.UTF8.GetBytes(key)))
                {
                    aes.Decrypt(nonceByte, encryptedByte, tagByte, textByte);
                    aes.Dispose();
                    return Encoding.UTF8.GetString(textByte);
                }
            }

            /// <summary>
            /// 将一个32位整数转为字节流
            /// </summary>
            /// <param name="num">要转换的整数</param>
            /// <returns>整数对应的字节流</returns>
            public static byte[] IntToByte(int num)
            {
                byte[] data = new byte[4];
                data[0] = (byte)(num & 0xFF);
                data[1] = (byte)((num & 0xFF00) >> 8);
                data[2] = (byte)((num & 0xFF0000) >> 16);
                data[3] = (byte)((num >> 24) & 0xFF);
                return data;
            }
        }
    }
}

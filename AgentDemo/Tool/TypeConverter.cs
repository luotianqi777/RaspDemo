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
                byte[] tagByte = new byte[16];
                byte[] nonceByte = new byte[12];
                new Random().NextBytes(nonceByte);
                byte[] textByte = Encoding.UTF8.GetBytes(text);
                byte[] encryptedByte = new byte[textByte.Length];
                using (AesGcm aes = new AesGcm(Encoding.UTF8.GetBytes(key)))
                {
                    aes.Encrypt(nonceByte, textByte, encryptedByte, tagByte);
                    tag = Convert.ToBase64String(tagByte);
                    nonce = Convert.ToBase64String(nonceByte);
                    return Convert.ToBase64String(encryptedByte);
                }
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

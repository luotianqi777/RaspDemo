using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AgentDemo
{
    public static partial class Tool
    {
        public static class XTypeConverter
        {
            public static string AESEncrypt(string text, string key, out string tag, out string nonce)
            {
                byte[] tagByte = new byte[16];
                byte[] nonceByte = new byte[12];
                new Random().NextBytes(nonceByte);
                byte[] textByte = Encoding.UTF8.GetBytes(text);
                byte[] encryptedByte = new byte[textByte.Length];
                using (AesCcm aes = new AesCcm(Encoding.UTF8.GetBytes(key)))
                {
                    aes.Encrypt(nonceByte, textByte, encryptedByte, tagByte);
                    tag = Convert.ToBase64String(tagByte);
                    nonce = Convert.ToBase64String(nonceByte);
                    return Convert.ToBase64String(encryptedByte);
                }
            }

            public static string AESDecrypt(string encryptedText, string key, string tag, string nonce)
            {
                byte[] tagByte = Convert.FromBase64String(tag);
                byte[] nonceByte =Convert.FromBase64String(nonce); 
                byte[] encryptedByte =Convert.FromBase64String(encryptedText);
                byte[] textByte = new byte[encryptedByte.Length];
                using (AesCcm aes = new AesCcm(Encoding.UTF8.GetBytes(key)))
                {
                    aes.Decrypt(nonceByte, encryptedByte, tagByte, textByte);
                    return Encoding.UTF8.GetString(textByte);
                }
            }

            public static string StrToBase64(string str)
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                return System.Convert.ToBase64String(data);
            }

            public static string Base64ToStr(string base64str)
            {
                byte[] data = System.Convert.FromBase64String(base64str);
                return Encoding.UTF8.GetString(data);
            }

            public static byte[] IntToByte(Int32 num)
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

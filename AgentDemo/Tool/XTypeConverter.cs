using Org.BouncyCastle.Crypto.Tls;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AgentDemo
{
    partial class Tool
    {
        public class XTypeConverter
        {

            #region AESEncrypt
            /// <summary>  
            /// AES加密  
            /// </summary>  
            /// <param name="text">要加密的明文</param>  
            /// <param name="tag">密钥</param>  
            /// <param name="nonce">向量</param>  
            /// <returns>Base64转码后的密文</returns>  
            // public static string AESEncrypt(string text, string tag, string nonce)
            // {
            //     byte[] plainBytes = Encoding.UTF8.GetBytes(text);
            //     byte[] bTag = new byte[32];
            //     Array.Copy(Encoding.UTF8.GetBytes(tag.PadRight(bTag.Length)), bTag, bTag.Length);
            //     byte[] bNonce = new byte[16];
            //     Array.Copy(Encoding.UTF8.GetBytes(nonce.PadRight(bNonce.Length)), bNonce, bNonce.Length);
            //     byte[] Cryptograph = null; // 加密后的密文  
            //     Rijndael Aes = Rijndael.Create();
            //     AesGcm aesGcm = new AesGcm(bTag);
            //     try
            //     {
            //         // 开辟一块内存流  
            //         using MemoryStream Memory = new MemoryStream();
            //         // 把内存流对象包装成加密流对象  
            //         using CryptoStream Encryptor = new CryptoStream(Memory,
            //          Aes.CreateEncryptor(bTag, bNonce),
            //          CryptoStreamMode.Write);
            //         // 明文数据写入加密流  
            //         Encryptor.Write(plainBytes, 0, plainBytes.Length);
            //         Encryptor.FlushFinalBlock();
            //         Cryptograph = Memory.ToArray();
            //     }
            //     catch
            //     {
            //         Cryptograph = null;
            //     }
            //     return System.Convert.ToBase64String(Cryptograph);
            // }
            #endregion

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
                    tag = Encoding.UTF8.GetString(tagByte);
                    nonce = Encoding.UTF8.GetString(nonceByte);
                    return Encoding.UTF8.GetString(encryptedByte);
                }
            }

            public static string AESDecrypt(string encryptedText, string key, string tag, string nonce)
            {
                byte[] tagByte =  Encoding.UTF8.GetBytes(tag);
                byte[] nonceByte = Encoding.UTF8.GetBytes(nonce);
                byte[] encryptedByte = Encoding.UTF8.GetBytes(encryptedText);
                byte[] textByte = new byte[encryptedText.Length];
                using (AesCcm aes = new AesCcm(Encoding.UTF8.GetBytes(key)))
                {
                    aes.Decrypt(nonceByte, encryptedByte, tagByte, textByte);
                    return Encoding.UTF8.GetString(textByte);
                }
            }

            #region AESDecrypt
            /// <summary>  
            /// AES解密  
            /// </summary>  
            /// <param name="Data">要被解密的密文</param>  
            /// <param name="Key">密钥</param>  
            /// <param name="Vector">向量</param>  
            /// <returns>UTF8解码后的明文</returns>  
            // public static string AESDecrypt(string Data, string Key, string Vector)
            // {
            //     byte[] encryptedBytes = System.Convert.FromBase64String(Data);
            //     byte[] bKey = new byte[32];
            //     Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            //     byte[] bVector = new byte[16];
            //     Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            //     byte[] original = null; // 解密后的明文  

            //     Rijndael Aes = Rijndael.Create();
            //     try
            //     {
            //         // 开辟一块内存流，存储密文  
            //         using MemoryStream Memory = new MemoryStream(encryptedBytes);
            //         // 把内存流对象包装成加密流对象  
            //         using CryptoStream Decryptor = new CryptoStream(Memory,
            //         Aes.CreateDecryptor(bKey, bVector),
            //         CryptoStreamMode.Read);
            //         // 明文存储区  
            //         using MemoryStream originalMemory = new MemoryStream();
            //         byte[] Buffer = new byte[1024];
            //         Int32 readBytes = 0;
            //         while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
            //         {
            //             originalMemory.Write(Buffer, 0, readBytes);
            //         }

            //         original = originalMemory.ToArray();
            //     }
            //     catch
            //     {
            //         original = null;
            //     }
            //     return Encoding.UTF8.GetString(original);
            // }
            #endregion

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

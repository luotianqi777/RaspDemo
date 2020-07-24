using Org.BouncyCastle.Crypto.Tls;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AgentDemo
{
    partial class Tool
    {
        public class Json
        {
            public static string AesEncrypt(string rawInput, byte[] key, byte[] iv)
            {
                if (string.IsNullOrEmpty(rawInput))
                {
                    return string.Empty;
                }

                if (key == null || iv == null || key.Length < 1 || iv.Length < 1)
                {
                    throw new ArgumentException("Key/Iv is null.");
                }

                using var rijndaelManaged = new RijndaelManaged()
                {
                    Key = key, // 密钥，长度可为128， 196，256比特位
                    IV = iv,  //初始化向量(Initialization vector), 用于CBC模式初始化
                    KeySize = 256,//接受的密钥长度
                    BlockSize = 128,//加密时的块大小，应该与iv长度相同
                    Mode = CipherMode.CBC,//加密模式
                    Padding = PaddingMode.PKCS7
                };
                using var transform = rijndaelManaged.CreateEncryptor(key, iv);
                var inputBytes = Encoding.UTF8.GetBytes(rawInput);//字节编码， 将有特等含义的字符串转化为字节流
                var encryptedBytes = transform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);//加密
                return Convert.ToBase64String(encryptedBytes);//将加密后的字节流转化为字符串，以便网络传输与储存。
            }

            public static string AesDecrypt(string encryptedInput, byte[] key, byte[] iv)
            {
                if (string.IsNullOrEmpty(encryptedInput))
                {
                    return string.Empty;
                }

                if (key == null || iv == null || key.Length < 1 || iv.Length < 1)
                {
                    throw new ArgumentException("Key/Iv is null.");
                }

                using var rijndaelManaged = new RijndaelManaged()
                {
                    Key = key,
                    IV = iv,
                    KeySize = 256,
                    BlockSize = 128,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                using var transform = rijndaelManaged.CreateDecryptor(key, iv);
                var inputBytes = Convert.FromBase64String(encryptedInput);
                var encryptedBytes = transform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                return Encoding.UTF8.GetString(encryptedBytes);
            }

            /// <summary>  
            /// AES加密  
            /// </summary>  
            /// <param name="Data">被加密的明文</param>  
            /// <param name="Key">密钥</param>  
            /// <param name="Vector">向量</param>  
            /// <returns>密文</returns>  
            public static string AESEncrypt(string Data, string Key, string Vector)
            {
                Byte[] plainBytes = Encoding.UTF8.GetBytes(Data);

                Byte[] bKey = new Byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
                Byte[] bVector = new Byte[16];
                Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

                Byte[] Cryptograph = null; // 加密后的密文  

                Rijndael Aes = Rijndael.Create();
                try
                {
                    // 开辟一块内存流  
                    using MemoryStream Memory = new MemoryStream();
                    // 把内存流对象包装成加密流对象  
                    using CryptoStream Encryptor = new CryptoStream(Memory,
                     Aes.CreateEncryptor(bKey, bVector),
                     CryptoStreamMode.Write);
                    // 明文数据写入加密流  
                    Encryptor.Write(plainBytes, 0, plainBytes.Length);
                    Encryptor.FlushFinalBlock();

                    Cryptograph = Memory.ToArray();
                }
                catch
                {
                    Cryptograph = null;
                }

                return Convert.ToBase64String(Cryptograph);
            }

            /// <summary>  
            /// AES解密  
            /// </summary>  
            /// <param name="Data">被解密的密文</param>  
            /// <param name="Key">密钥</param>  
            /// <param name="Vector">向量</param>  
            /// <returns>明文</returns>  
            public static string AESDecrypt(string Data, string Key, string Vector)
            {
                Byte[] encryptedBytes = Convert.FromBase64String(Data);
                Byte[] bKey = new Byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
                Byte[] bVector = new Byte[16];
                Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

                Byte[] original = null; // 解密后的明文  

                Rijndael Aes = Rijndael.Create();
                try
                {
                    // 开辟一块内存流，存储密文  
                    using MemoryStream Memory = new MemoryStream(encryptedBytes);
                    // 把内存流对象包装成加密流对象  
                    using CryptoStream Decryptor = new CryptoStream(Memory,
                    Aes.CreateDecryptor(bKey, bVector),
                    CryptoStreamMode.Read);
                    // 明文存储区  
                    using MemoryStream originalMemory = new MemoryStream();
                    Byte[] Buffer = new Byte[1024];
                    Int32 readBytes = 0;
                    while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                    {
                        originalMemory.Write(Buffer, 0, readBytes);
                    }

                    original = originalMemory.ToArray();
                }
                catch
                {
                    original = null;
                }
                return Encoding.UTF8.GetString(original);
            }

            public static string StrToBase64(string str)
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                return Convert.ToBase64String(data);
            }

            public static string Base64ToStr(string base64str)
            {
                byte[] data = Convert.FromBase64String(base64str);
                return Encoding.UTF8.GetString(data);
            }

        }
    }
}

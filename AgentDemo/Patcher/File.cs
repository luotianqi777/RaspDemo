using System;
using System.IO;
using System.Net;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    public class File
    {

        /// <summary>
        /// 检测info是否有文件读取漏洞
        /// </summary>
        /// <param name="info">检测的信息</param>
        static void CheckFileRead(string info)
        {
            // 发送检测请求
            Checker.SendCheckRequest("file_read");
            // 检测漏洞(IAST)
            Checker.Check(new Checker.FileRead(), info, BasePatcher.GetStackTrace());
        }

        [HarmonyPatch(typeof(WebClient))]
        [HarmonyPatch(nameof(WebClient.DownloadData), new Type[] { typeof(string) })]
        class DownloadData:BasePatcher
        {
            protected static bool Prefix(string address)
            {
                CheckFileRead(address);
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch(nameof(File.OpenRead), new Type[] { typeof(string) })]
        class OpenRead:BasePatcher
        {
            static bool Prefix(string path)
            {
                CheckFileRead(path);
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch(nameof(File.ReadAllBytes), new Type[] { typeof(string) })]
        class ReadAllBytes:BasePatcher
        {
            static bool Prefix(string path)
            {
                CheckFileRead(path);
                return true;
            }
        }
    
        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch(nameof(File.ReadAllText), new Type[] { typeof(string) })]
        class ReadAllText:BasePatcher
        {
            static bool Prefix(string path)
            {
                CheckFileRead(path);
                return true;
            }
        }
    
        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch(nameof(File.Delete), new Type[] { typeof(string) })]
        class Delete:BasePatcher
        {
            static bool Prefix(string path)
            {
                CheckFileRead(path);
                return true;
            }
        }

    }
}

using System;
using System.IO;
using System.Net;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    public class Read
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
            Checker.Check(new Checker.FileRead(), info, "文件下载漏洞调用栈");
        }

        [HarmonyPatch(typeof(WebClient))]
        [HarmonyPatch(nameof(WebClient.DownloadData), new Type[] { typeof(string) })]
        class DownloadData
        {
            protected static bool Prefix(string address)
            {
                CheckFileRead(address);
                return true;
            }
        }

        [HarmonyPatch(typeof(File))]
        [HarmonyPatch(nameof(File.OpenRead), new Type[] { typeof(string) })]
        class OpenRead
        {
            static bool Prefix(string path)
            {
                CheckFileRead(path);
                return true;
            }
        }

        [HarmonyPatch(typeof(File))]
        [HarmonyPatch(nameof(File.ReadAllBytes), new Type[] { typeof(string) })]
        class ReadAllBytes
        {
            static bool Prefix(string path)
            {
                CheckFileRead(path);
                return true;
            }
        }
    
        [HarmonyPatch(typeof(File))]
        [HarmonyPatch(nameof(File.Delete), new Type[] { typeof(string) })]
        class Delete
        {
            static bool Prefix(string path)
            {
                CheckFileRead(path);
                return true;
            }
        }

    }
}

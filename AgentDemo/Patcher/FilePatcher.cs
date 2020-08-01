using System;
using System.Net;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    [HarmonyPatch(typeof(WebClient))]
    [HarmonyPatch("DownloadData",new Type[] { typeof(string)})]
    class FilePatcher
    {
        static bool Prefix(string address)
        {
            var request = XTool.HttpHelper.GetCurrentHttpRequest();
            // 检测漏洞(IAST)
            // TODO:还没有RASP检测逻辑
            CheckLogic.Check(url => true, request, address, "文件下载漏洞调用栈");
            // 发送检测请求
            CheckLogic.SendCheckRequest(request, "file_read");
            return true;
        }
    }
}

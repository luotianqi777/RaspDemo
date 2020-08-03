using System;
using System.IO;
using System.Net;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    public class File
    {

        [HarmonyPatch(typeof(WebClient))]
        [HarmonyPatch(nameof(WebClient.DownloadData), new Type[] { typeof(string) })]
        class DownloadData : BasePatcher
        {
            protected static bool Prefix(string address)
            {
                Checker.SendCheckRequest("file_read");
                Checker.Check(new Checker.FileRead(), address, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch("OpenRead", new Type[] { typeof(string) })]
        class OpenRead : BasePatcher
        {
            protected static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_read");
                Checker.Check(new Checker.FileRead(), path, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch("ReadAllBytes", new Type[] { typeof(string) })]
        class ReadAllBytes : BasePatcher
        {
            protected static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_read");
                Checker.Check(new Checker.FileRead(), path, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch("ReadAllText", new Type[] { typeof(string) })]
        class ReadAllText : BasePatcher
        {
            protected static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_read");
                Checker.Check(new Checker.FileRead(), path, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch("Delete", new Type[] { typeof(string) })]
        class Delete : BasePatcher
        {
            protected static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_read");
                Checker.Check(new Checker.FileRead(), path, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch("Create", new Type[] { typeof(string) })]
        class Create : BasePatcher
        {
            protected static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_upload");
                // Checker.Check(new Checker.FileRead(), path, GetStackTrace());
                Checker.Check(new Checker.FileUpload(), path, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(System.IO.File))]
        [HarmonyPatch("Copy", new Type[] { typeof(string),typeof(string) })]
        class Copy : BasePatcher
        {
            protected static bool Prefix(string sourceFileName, string destFileName)
            {
                if (destFileName.Contains(sourceFileName))
                {
                    Checker.SendCheckRequest("file_upload");
                    // Checker.Check(new Checker.FileRead(), path, GetStackTrace());
                    Checker.Check(new Checker.FileUpload(), destFileName, GetStackTrace());
                }
                return true;
            }
        }



    }
}

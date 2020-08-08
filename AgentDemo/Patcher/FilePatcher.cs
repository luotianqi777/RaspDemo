using System;
using System.IO;
using System.Net;
using System.Text;
using HarmonyLib;

namespace AgentDemo.Patcher
{
    public class FilePatcher
    {

        #region FileRead
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

        [HarmonyPatch(typeof(File))]
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

        [HarmonyPatch(typeof(File))]
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

        [HarmonyPatch(typeof(File))]
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

        #endregion

        #region FileUpload

        [HarmonyPatch(typeof(File))]
        [HarmonyPatch("Create", new Type[] { typeof(string) })]
        class Create : BasePatcher
        {
            protected static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_upload");
                Checker.Check(new Checker.FileUpload(), path, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(File))]
        [HarmonyPatch("Copy", new Type[] { typeof(string),typeof(string) })]
        class Copy : BasePatcher
        {
            protected static bool Prefix(string sourceFileName, string destFileName)
            {
                if (destFileName.Contains(sourceFileName))
                {
                    Checker.SendCheckRequest("file_upload");
                    Checker.Check(new Checker.FileUpload(), destFileName, GetStackTrace());
                }
                return true;
            }
        }

        #endregion

        #region FileWrite

        // [HarmonyPatch(typeof(StreamWriter))]
        // [HarmonyPatch(new Type[] { typeof(string), typeof(bool), typeof(Encoding), typeof(int) })]
        class Writer:BasePatcher
        {
            public static bool Prefix()
            {
                Checker.SendCheckRequest("file_write");
                // Checker.Check(new Checker.FileRead(), path, GetStackTrace());
                return true;
            }
        }

        [HarmonyPatch(typeof(File))]
        [HarmonyPatch(nameof(File.WriteAllLines), new Type[] { typeof(string), typeof(string[]) })]
        class WriteAllLines : BasePatcher {
            public static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_write", "file_read");
                Checker.Check(new Checker.FileRead(), path, GetStackTrace(),type=> {
                    return type.Equals("file_read") ? "file_write" : type;
                });
                return true;
            }
        }

        [HarmonyPatch(typeof(File))]
        [HarmonyPatch(nameof(File.WriteAllText), new Type[] { typeof(string), typeof(string) })]
        class WriteAllText : BasePatcher {
            public static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_write", "file_read");
                Checker.Check(new Checker.FileRead(), path, GetStackTrace(),type=> {
                    return type.Equals("file_read") ? "file_write" : type;
                });
                return true;
            }
        }

        [HarmonyPatch(typeof(File))]
        [HarmonyPatch(nameof(File.WriteAllBytes), new Type[] { typeof(string), typeof(byte[]) })]
        class WriteAllBytes : BasePatcher {
            public static bool Prefix(string path)
            {
                Checker.SendCheckRequest("file_write", "file_read");
                Checker.Check(new Checker.FileRead(), path, GetStackTrace(),type=> {
                    return type.Equals("file_read") ? "file_write" : type;
                });
                return true;
            }
        }

        #endregion

    }
}

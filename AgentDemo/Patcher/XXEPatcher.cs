using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using HarmonyLib;
using Newtonsoft.Json.Schema;

namespace AgentDemo.Patcher
{

    class XDocumentPatcher
    {

        [HarmonyPatch(typeof(XDocument))]
        [HarmonyPatch(nameof(XDocument.Parse), new Type[] { typeof(string) })]
        class ParseXml : BasePatcher
        {
            static bool Prefix(string text)
            {
                Checker.SendCheckRequest("xxe");
                Checker.Check(new Checker.XXE(), text, GetStackTrace());
                return true;
            }
        }
    }

    class XmlDocumentPatcher
    {

        [HarmonyPatch(typeof(XmlDocument))]
        [HarmonyPatch(nameof(XmlDocument.Load), new Type[] { typeof(XmlReader) })]
        class LoadXml : BasePatcher
        {
            static bool Prefix(XmlReader reader)
            {
                try
                {
                    var xml = reader.ReadContentAsString();
                    Checker.SendCheckRequest("xxe");
                    Checker.Check(new Checker.XXE(), xml, GetStackTrace());
                }
                catch (Exception e)
                {
                    Debuger.WriteLine($"xxe解析错误:{e.Message}");
                }
                return true;
            }
        }

        // [HarmonyPatch(typeof(XmlLoader))]
        // [HarmonyPatch("Load", new Type[] {typeof(XmlDocument), typeof(XmlReader),typeof(bool)})]
        [XPatch("System.Xml", "System.Xml.XmlLoader", "Load", new Type[] { typeof(XmlDocument), typeof(XmlReader), typeof(bool) })]
        class Loader : BasePatcher
        {
            protected static bool Prefix(XmlReader reader)
            {
                try
                {
                    var xml = reader.ReadContentAsString();
                    Checker.SendCheckRequest("xxe");
                    Checker.Check(new Checker.XXE(), xml, GetStackTrace());
                }
                catch (Exception e)
                {
                    Debuger.WriteLine($"xxe解析错误:{e.Message}");
                }
                return true;
            }
        }

    }
}

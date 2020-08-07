using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class XXEController : Controller
    {
        [HttpGet]
        public IActionResult Index(string xml)
        {
            try
            {
                UseXDocument(xml);
                // UseXmlDocument(xml);
            }
            catch (Exception e)
            {
                Debuger.WriteLine($"解析失败: {e.Message}");
            }
            return View();
        }

        private void UseXmlDocument(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var nodes = doc.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                Debuger.WriteLine($"name:{node.Name}, value:{node.InnerText}");
            }

        }

        private void UseXDocument(string xml)
        {
            var doc = XDocument.Parse(xml);
            var node = doc.Root;
            Debuger.WriteLine(node.Value);
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgentDemo.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;

namespace AgentDemo.Json.Tests
{
    [TestClass()]
    public class JsonMangerTests
    {
        [TestMethod()]
        public void SendJsonTest()
        {
            string message = "test_123_ABC";
            string key = "LATNLOFPVVDGAEVG";
            var encryptMessage = Tool.XTypeConverter.AESEncrypt(message,key,out string tag, out string nonce);
            var str = Tool.XTypeConverter.AESDecrypt(encryptMessage, key, tag, nonce);
            Console.WriteLine(str);
            Assert.AreEqual(str, message);
        }
    }
}
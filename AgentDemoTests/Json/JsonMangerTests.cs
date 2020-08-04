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
            int size = 3142411;
            byte[] bytes = XTool.TypeConverter.IntToByte(size);
            int otherSize = XTool.TypeConverter.ByteToInt(bytes);
            Assert.AreEqual(size, otherSize);
        }
    }
}
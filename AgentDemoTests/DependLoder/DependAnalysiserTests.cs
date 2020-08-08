using Microsoft.VisualStudio.TestTools.UnitTesting;
using IastNetAgent.DependLoder;
using System;
using System.Collections.Generic;
using System.Text;

namespace IastNetAgent.DependLoder.Tests
{
    [TestClass()]
    public class DependAnalysiserTests
    {
        [TestMethod()]
        public void GetPackageInfoSetTest()
        {
            foreach ( var info in DependAnalysiser.GetPackageInfos())
            {
                Debuger.WriteLine($"key: {info.Key}");
                info.Value.ForEach(e => Debuger.WriteLine(e));
            }
        }
    }
}
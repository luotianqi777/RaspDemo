using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgentDemo.DependLoder;
/* ==============================================================================
* 功能描述：DependAnalysiserTests  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/7 20:14:35
* ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.DependLoder.Tests
{
    [TestClass()]
    public class DependAnalysiserTests
    {
        [TestMethod()]
        public void GetPackageInfoSetTest()
        {
            foreach (PackageInfo info in DependAnalysiser.GetPackageInfoSet())
            {
                Debuger.WriteLine(info);
            }
        }
    }
}
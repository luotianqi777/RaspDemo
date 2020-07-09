/* ==============================================================================
* 功能描述：MyPatchAttribute  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/4 11:36:26
* ==============================================================================*/
using AgentDemo.DependLoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;

namespace AgentDemo.Patcher
{

    /// <summary>
    /// 派生自HarmontPatch
    /// 可以更方便的处理依赖
    /// </summary>
    public class MyPatchAttribute : HarmonyPatch
    {

        public MyPatchAttribute(string packageName,
                                string moudleName,
                                string className,
                                string methodName,
                                Type[] types)
        {
            info.declaringType = GetClassType(packageName, moudleName, className);
            bool isPatch = info.declaringType != null;
            info.methodName = isPatch ? methodName : null;
            info.argumentTypes = isPatch ? types : null;
        }

        /// <summary>
        /// 获取要Patch的Class类型
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <param name="className">Class名</param>
        /// <returns>Class类型，返回null说明找不到路径</returns>
        private static Type GetClassType(string packageName, string moudleName, string className)
        {
            // basePath: 项目文件所在路径
            const string basePath = @"bin\Debug\netcoreapp3.1\";
            // 获取包所对应的dll路径
            Dictionary<PackageInfo, List<string>> keyValuePairs = DependAnalysiser.GetPackageInfos(basePath + "RaspDemo.deps.json");
            keyValuePairs.TryGetValue(new PackageInfo(packageName), out List<string> dllPaths);
            // 成功获取路径则读取程序集，否则返回null代表找不到路径，应当停止hook该方法
            if (dllPaths == null || dllPaths.Count == 0)
            {
                Debuger.WriteLine($"package {packageName} not find");
                return null;
            }
            string dllPath = basePath + dllPaths[0];
            if (!File.Exists(dllPath))
            {
                Debuger.WriteLine($"file {dllPath} not find");
                return null;
            }
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                Type type = assembly.GetType($"{packageName}.{moudleName}.{className}");
                return type;
            }
            catch(Exception e)
            {
                Debuger.WriteLine(e.Source + e.Message + e.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// 获取要Patch的Class类型
        /// </summary>
        /// <typeparam name="T">Patcher的类型</typeparam>
        /// <returns>Patch的Class类型</returns>
        public static Type GetPatchedClassType<T>()where T:BasePatcher {
            MyPatchAttribute attribute =  (MyPatchAttribute)GetCustomAttribute(typeof(T), typeof(MyPatchAttribute));
            return attribute.info.declaringType;
        }

    }
}

using AgentDemo.DependLoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using System.Runtime.Loader;

namespace AgentDemo.Patcher
{

    /// <summary>
    /// 派生自HarmontPatch
    /// 可以更方便的处理依赖
    /// </summary>
    public class XPatchAttribute : HarmonyPatch
    {
        public string FullClassName { get; }

        public XPatchAttribute(string packageName,
                                string className,
                                string methodName,
                                Type[] types)
        {
            Type classType = GetClassType(packageName, className);
            if (classType == null)
            {
                info = null;
                Debuger.WriteLine($"class {className} not find from package {packageName}");
            }
            else
            {
                info.declaringType = classType;
                if (methodName == null) info.methodType = MethodType.Constructor;
                info.methodName = methodName;
                info.argumentTypes = types;
            }
        }

        /// 获取要Patch的Class类型
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <param name="className">类名</param>
        /// <returns>Class类型，返回null说明找不到路径</returns>
        private static Type GetClassType(string packageName, string className)
        {
            try
            {
                // 获取包所对应的dll路径
                DependAnalysiser.GetPackageInfos().TryGetValue(new PackageInfo(packageName), out List<string> dllPaths);
                // 成功获取路径则读取程序集，否则返回null代表找不到文件或类，应当停止hook该方法
                string dllPath = dllPaths?[0] ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{packageName}.dll");
                Assembly assembly = Assembly.LoadFrom(dllPath);
                return assembly?.GetType(className);
                // return Assembly.LoadFrom(dllPath)?.GetType(className);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取要Patch的Class类型
        /// </summary>
        /// <typeparam name="T">Patcher的类型</typeparam>
        /// <returns>Patch的Class类型</returns>
        public static Type GetPatchedClassType<T>() where T : BasePatcher
        {
            XPatchAttribute attribute = (XPatchAttribute)GetCustomAttribute(typeof(T), typeof(XPatchAttribute));
            return attribute.info.declaringType;
        }

    }
}

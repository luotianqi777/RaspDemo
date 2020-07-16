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
using System.Runtime.Loader;

namespace AgentDemo.Patcher
{

    /// <summary>
    /// 派生自HarmontPatch
    /// 可以更方便的处理依赖
    /// </summary>
    public class MyPatchAttribute : HarmonyPatch
    {
        public string FullClassName { get; }

        public MyPatchAttribute(string packageName,
                                string className,
                                string methodName,
                                Type[] types)
        {
            Type classType = GetClassType(packageName, className);
            if (classType == null)
            {
                info = null;
                Debuger.WriteLine($"can't find class {className} from package {packageName}");
            }
            else
            {
                info.declaringType = classType;
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
                return Assembly.LoadFrom(dllPaths[0])?.GetType(className);
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
            MyPatchAttribute attribute = (MyPatchAttribute)GetCustomAttribute(typeof(T), typeof(MyPatchAttribute));
            return attribute.info.declaringType;
        }

    }
}

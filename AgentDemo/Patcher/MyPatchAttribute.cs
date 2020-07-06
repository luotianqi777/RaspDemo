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
using System.Text;

namespace AgentDemo.Patcher
{

    /// <summary>
    /// 派生自HarmontPatch
    /// 可以更方便的处理依赖
    /// </summary>
    public class MyPatchAttribute : HarmonyLib.HarmonyPatch
    {
        /// <summary>
        /// 标识是否应该被patch
        /// </summary>
        private readonly bool isPatch;

        public MyPatchAttribute(string packageName,
                                string className,
                                string methodName,
                                Type[] types) 
        {
            info.declaringType = GetClassType(packageName, className);
            isPatch = info.declaringType != null;
            info.methodName = methodName;
            info.argumentTypes = types;
        }

        /// <summary>
        /// 获取要Patch的Class类型
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <param name="className">Class名</param>
        /// <returns>Class类型</returns>
        private static Type GetClassType (string packageName, string className)
        {
            // TODO: 这个地方要通过分析项目文件找出来依赖所对应的dll路径，根据packageName去匹配dllPath
            string dllPath = @"bin\Debug\netcoreapp3.1\MySql.Data.dll";
            if (!File.Exists(dllPath))
            {
                Console.WriteLine(dllPath + " not exists");
                return null;
            }
            Assembly assembly = Assembly.LoadFrom(dllPath);
            Type type = assembly.GetType($"{packageName}.{className}");
            return type;
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

        /// <summary>
        /// 该类是否应该被Patch
        /// </summary>
        /// <typeparam name="T">Patcher的类型</typeparam>
        /// <returns>true:应该被Patch</returns>
        public static bool IsPatch<T>() where T : BasePatcher
        {
            MyPatchAttribute attribute =  (MyPatchAttribute)GetCustomAttribute(typeof(T), typeof(MyPatchAttribute));
            return attribute.isPatch;
        }
    }
}

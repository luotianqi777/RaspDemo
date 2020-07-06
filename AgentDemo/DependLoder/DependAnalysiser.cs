/* ==============================================================================
* 功能描述：分析项目中的依赖
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/3 15:18:48
* ==============================================================================*/
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

/// <summary>
/// 用于分析项目中的依赖
/// </summary>
namespace AgentDemo.DependLoder
{
    public class DependAnalysiser
    {

        /// <summary>
        /// 默认项目依赖文件路径
        /// </summary>
        private static string dependFilePath = "*.deps.json";

        /// <summary>
        /// 项目依赖库的集合
        /// </summary>
        private static HashSet<PackageInfo> packageInfoSet = null;

        /// <summary>
        /// 获取项目依赖库的集合
        /// </summary>
        public static HashSet<PackageInfo> GetPackageInfoSet()
        {
            if (packageInfoSet == null)
            {
                packageInfoSet = AnalysisDepend(dependFilePath);
            }
            return packageInfoSet;
        }

        /// <summary>
        /// 设置依赖文件路径
        /// </summary>
        /// <param name="filePath">依赖文件路径</param>
        public static void SetDepenFilePath(string filePath)
        {
            dependFilePath = filePath;
        }

        /// <summary>
        /// 分析依赖文件，将依赖信息放到一个集合
        /// </summary>
        /// <param name="filePath">依赖文件路径</param>
        /// <returns>依赖集合</returns>
        private static HashSet<PackageInfo> AnalysisDepend(string filePath)
        {
            packageInfoSet = new HashSet<PackageInfo>();
            try
            {
                string fileContext = File.ReadAllText(filePath);
                if (String.IsNullOrEmpty(fileContext))
                {
                    throw new Exception("file open failure");
                }
                int startIndex = 0, endIndex = 0;
                while (true)
                {
                    startIndex = fileContext.IndexOf("dependencies", endIndex);
                    if (startIndex == -1) { break; }
                    endIndex = fileContext.IndexOf('}', startIndex);
                    /*
                     * subString:
                     * "dependencies":{
                     *   "package name":"version number",
                     *   "package name":"version number",
                     *   ...
                     */
                    string subString = fileContext[startIndex..endIndex]; ;
                    // 简化正则："w+(.w+)*": "d+(.d+)*"
                    string regexString = @"""\w+([.]\w+)*"": *""\d+([.]\d+)*""";
                    Regex regex = new Regex(regexString);
                    int regexStartIndex = 0;
                    while (true)
                    {
                        Match match = regex.Match(subString, regexStartIndex);
                        if (!match.Success) { break; }
                        regexStartIndex += match.Length;
                        /*
                         * keyValuePair:
                         * ^"package name":"version number"$
                         */
                        string keyValuePair = match.Value.Replace("\"",string.Empty);
                        int midIndex = keyValuePair.IndexOf(": ");
                        string packageName = keyValuePair.Substring(0, midIndex);
                        string versionNumber = keyValuePair.Substring(midIndex+2);
                        packageInfoSet.Add(new PackageInfo(packageName, versionNumber));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
            return packageInfoSet;
        }

    }
}

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
        /// 项目依赖库的集合
        /// </summary>
        private static Dictionary<PackageInfo, List<string>> packageInfos = null;

        /// <summary>
        /// 分析依赖文件，将依赖信息放到一个集合
        /// <param name="filePath">依赖文件所在文件路径</param>
        /// <returns>依赖集合</returns>
        public static Dictionary<PackageInfo, List<string>> GetPackageInfos(string filePath)
        {
            if (packageInfos != null) return packageInfos;
            packageInfos = new Dictionary<PackageInfo, List<string>>();
            try
            {
                Debuger.WriteLine($"loading file {filePath}");
                string fileContext = File.ReadAllText(filePath);
                string regexString = @"""[\w|.]+/[\d|.]+"":";
                Regex regex = new Regex(regexString);
                int regexStartIndex = 0;
                while (true)
                {
                    // 正则匹配 package info
                    Match match = regex.Match(fileContext, regexStartIndex);
                    if (!match.Success) { break; }
                    // 储存 package info
                    string packageInfoString = match.Value[1..match.Value.IndexOf('"', 2)];
                    int midIndex = packageInfoString.IndexOf('/');
                    PackageInfo packageInfo = new PackageInfo(
                        packageInfoString.Substring(0, midIndex),
                        packageInfoString.Substring(midIndex + 1));
                    // 截取package info的json数据在字符串中的起始位置
                    int startIndex = match.Index + match.Length;
                    int endIndex = GetNextBracketsIndex(fileContext, startIndex);
                    // 更新下一次正则查找起始位置
                    regexStartIndex = endIndex;
                    // 获取compile的json数据
                    int compileStartIndex = fileContext.IndexOf("compile",startIndex);
                    if (compileStartIndex == -1 || compileStartIndex > endIndex) continue;
                    string jsonString = fileContext[compileStartIndex..endIndex];
                    // 获取dll路径
                    List<string> dllPathString = GetDllPaths(jsonString);
                    // 将dll路径与包名作为键值对存储起来
                    if (dllPathString.Count > 0)
                    {
                        packageInfos.Add(packageInfo, dllPathString);
                    }
                }
                Debuger.WriteLine($"analysis success! {packageInfos.Count} package find!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
            return packageInfos;
        }


        /// <summary>
        /// 获取一个字符串中从startIndex位置起下一个{所对应}的位置
        /// 比如{{}}，应返回第二个}的位置；而{}}应返回第一个}的位置；{{}则应返回-1
        /// </summary>
        /// <param name="context">要查找的字符串</param>
        /// <param name="startIndex">起始查找位置</param>
        /// <returns>能找到匹配的}返回位置下标，找不到返回-1</returns>
        private static int GetNextBracketsIndex(string context, int startIndex)
        {
            int count = 0;
            Regex regex = new Regex("[{|}]");
            do
            {
                Match match = regex.Match(context, startIndex);
                if (!match.Success) return -1;
                switch (match.Value)
                {
                    case "{":count++; break;
                    case "}":count--; break;
                    default:break;
                }
                startIndex = match.Index + match.Length;
            } while (count != 0);
            return startIndex;
        }

        /// <summary>
        /// 从一段json字符串中获取dll路径信息
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <returns>dll路径列表</returns>
        private static List<string> GetDllPaths(string jsonString)
        {
            List<string> dllPath = new List<string>();
            Regex regex = new Regex(@"(\w+[.])+dll");
            int startIndex = 0;
            while (true)
            {
                Match match = regex.Match(jsonString, startIndex);
                if (!match.Success) break;
                startIndex = match.Index + match.Length;
                dllPath.Add(match.Value);
            }
            return dllPath;
        }

    }
}

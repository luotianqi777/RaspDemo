/* ==============================================================================
* 功能描述：包信息类
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/3 17:08:44
* ==============================================================================*/

using System;
using System.Windows.Markup;

namespace AgentDemo.DependLoder
{
    /// <summary>
    /// 包信息类
    /// </summary>
    public class PackageInfo
    {
        /// <summary>
        /// 包名
        /// </summary>
        public string PackageName { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNumber { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <param name="versionNumber">版本号</param>
        public PackageInfo(string packageName, string versionNumber)
        {
            PackageName = packageName??throw new Exception("packge name can't be null");
            VersionNumber = versionNumber;
        }

        /// <summary>
        /// 构造，省略版本号
        /// </summary>
        /// <param name="packageName">包名</param>
        public PackageInfo(string packageName) : this(packageName, "1.0") { }

        /// <summary>
        /// 是否对比版本号
        /// true:对比
        /// false:不对比
        /// </summary>
        private static readonly bool isCompareVersion = false;

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            PackageInfo dependData = (PackageInfo)obj;
            return PackageName.Equals(dependData.PackageName)
                   && (!isCompareVersion || VersionNumber.Equals(dependData.VersionNumber));
        }

        public override int GetHashCode()
        {
            return (isCompareVersion ? PackageName + VersionNumber : PackageName).GetHashCode();
        }

        public override string ToString()
        {
            if (!isCompareVersion)
            {
                return PackageName;
            }
            else
            {
                return $"Package Name: {PackageName}, Version Number: {VersionNumber}";
            }
        }
    }
}

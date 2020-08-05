using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace AgentDemo
{
    public class AgentConfig
    {
        public static string AesTag;
        public static string AesNonce;
        public static int Port { get; set; }
        public static string IP { get; set; }
        public static string LocalIP { get; set; }
        public static string AgentID { get; set; }
        public static string AesKey { get; set; }
        private static int timeOut = 1000 * 5;
        public static int TimeOut { get { return timeOut; } set { timeOut = value; } }
        public static bool DEBUG { get; set; }
        // 是否拦截漏洞
        public static bool BLOCK { get; set; }
        public static string AgentKey { get; set; }

        static AgentConfig()
        {
            if (!ConfigureManger.ReadFromFile())
            {
                LocalIP = GetLocalIP();
                TimeOut = 30 * 1000;
                DEBUG = true;
                BLOCK = true;
                AgentKey = "VG9tY2F0Oy87MTkyLjE2OC4xNzIuMjM5OzkwOTA7T1JWS1pWVE9aUFBZT05aRjtNWEZBT1RNVFVSQU5ZQVBS";
            }
            AnalysisAgentKey();
        }

        /// <summary>
        /// 分析AgentKey
        /// </summary>
        private static void AnalysisAgentKey()
        {
            var keys = Encoding.UTF8.GetString(Convert.FromBase64String(AgentKey)).Split(';');
            IP = keys[2];
            Port = int.Parse(keys[3]);
            AesKey = keys[4];
            AgentID = keys[5];
            ConfigureManger.SaveToFile();
        }

        #region GetLocalIP
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        protected static string GetLocalIP()
        {
            foreach (var address in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
            throw new Exception("心跳信息创建失败：找不到本地ipv4");
        }
        #endregion
    }
}

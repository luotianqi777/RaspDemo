using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AgentDemo
{
    public class AgentConfig
    {
        public string AesTag;
        public string AesNonce;
        public int Port { get; set; }
        public int LocalPort { get; set; }
        public string IP { get; set; }
        public string LocalIP { get; set; }
        public string AgentID { get; set; }
        public string AesKey { get; set; }
        private int timeOut = 1000 * 5;
        public int TimeOut { get { return timeOut; } set { timeOut = value; } }
        public bool DEBUG { get; set; }
        // 是否拦截漏洞
        public bool BLOCK { get; set; }
        public override string ToString()
        {
            return $"AgentID:{AgentID}, AesKey:{AesKey}, AesTag:{AesTag}, AesNonce:{AesNonce}";
        }

        private static AgentConfig agentConfig = null;

        /// <summary>
        /// 获取Agent配置信息
        /// TODO: 从配置文件中读取信息
        /// </summary>
        /// <returns>Agent配置信息</returns>
        public static AgentConfig GetInstance()
        {
            agentConfig ??= new AgentConfig()
            {
                Port = 9090,
                LocalPort = 5000,
                IP = "192.168.172.239",
                LocalIP = GetLocalIP(),
                TimeOut = 30 * 1000,
                AgentID = "YSZLFIPIOISCWLXW",
                AesKey = "KHHAJT1OCEDVSOTY",
                DEBUG = true,
                BLOCK = false
            };
            return agentConfig;
        }

        #region GetLocalIP
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        private static string GetLocalIP()
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

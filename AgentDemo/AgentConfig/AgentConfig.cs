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
        public static string AesTag;
        public static string AesNonce;
        public static int Port { get; set; }
        public static int LocalPort { get; set; }
        public static string IP { get; set; }
        public static string LocalIP { get; set; }
        public static string AgentID { get; set; }
        public static string AesKey { get; set; }
        private static int timeOut = 1000 * 5;
        public static int TimeOut { get { return timeOut; } set { timeOut = value; } }
        public static bool DEBUG { get; set; }
        // 是否拦截漏洞
        public static bool BLOCK { get; set; }
        public override string ToString()
        {
            return $"AgentID:{AgentID}, AesKey:{AesKey}, AesTag:{AesTag}, AesNonce:{AesNonce}";
        }

        static AgentConfig()
        {
            Port = 9090;
            LocalPort = 5000;
            IP = "192.168.172.239";
            LocalIP = GetLocalIP();
            TimeOut = 30 * 1000;
            DEBUG = true;
            BLOCK = false;
            SetAgentKey("VG9tY2F0Oy87MTkyLjE2OC4xNzIuMjM5OzkwOTA7WU9RTVpZVkFTUEwxWU9IVztJQ1pGT0FCUEhaVVdWRldV");
        }

        private static void SetAgentKey(string key)
        {
            key = Encoding.UTF8.GetString(Convert.FromBase64String(key));
            var keys = key.Split(';');
            AesKey = keys[4];
            AgentID = keys[5];
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

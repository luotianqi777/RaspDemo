using System;
using System.Collections.Generic;
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
        public override string ToString()
        {
            return $"AgentID:{AgentID}, AesKey:{AesKey}, AesTag:{AesTag}, AesNonce:{AesNonce}";
        }

        /// <summary>
        /// 获取Agent配置信息
        /// TODO: 从配置文件中读取信息
        /// </summary>
        /// <returns>Agent配置信息</returns>
        public static AgentConfig GetInstance()
        {
            return new AgentConfig()
            {
                Port = 9090,
                LocalPort = 5000,
                IP = "192.168.172.239",
                LocalIP = "192.168.172.146",
                TimeOut = 30 * 1000,
                AgentID = "DMHUEHFXSQADARKH",
                AesKey = "WPOIVXHUZINJRDQC",
                DEBUG = true,
            };
        }
    }
}

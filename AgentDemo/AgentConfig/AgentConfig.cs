using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public class AgentConfig
    {
        public string AesTag;
        public string AesNonce;
        public string IP { get; set; }
        public int Port { get; set; }
        public string AgentID { get; set; }
        public string AesKey { get; set; }
        private int timeOut = 1000 * 5;
        public int TimeOut { get { return timeOut; } set { timeOut = value; } }
        public override string ToString()
        {
            return $"AgentID:{AgentID}, AesKey:{AesKey}, AesTag:{AesTag}, AesNonce:{AesNonce}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Json
{
    public partial class XJson
    {

        public class AgentJson : XJson.JsonBase
        {
            public int TimeOut { get; set; }
            public string AgentKey { get; set; }
            public string LocalIP { get; set; }
            public bool DEBUG { get; set; }
            public bool BLOCK { get; set; }

            public static AgentJson GetInstance()
            {
                return new AgentJson
                {
                    TimeOut = AgentConfig.TimeOut,
                    AgentKey = AgentConfig.AgentKey,
                    BLOCK = AgentConfig.BLOCK,
                    DEBUG = AgentConfig.DEBUG,
                    LocalIP = AgentConfig.LocalIP
                };
            }
        }
    }
}

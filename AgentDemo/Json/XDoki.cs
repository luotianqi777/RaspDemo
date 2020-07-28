using Newtonsoft.Json;

namespace AgentDemo.Json
{
    public class XDoki
    {

        #region Sender
        public class Sender : XJsonData.XMsg
        {
            [JsonProperty("client_info")]
            public XClientInfo ClientInfo { get; set; }
            [JsonProperty("cmd")]
            public int Cmd { get; set; }

            public class XClientInfo : XJsonBase
            {
                [JsonProperty("version_pocs")]
                public int VersionPocs { get; set; }
                [JsonProperty("version_main")]
                public int VersionMain { get; set; }
                [JsonProperty("ip")]
                public string Ip { get; set; }
                [JsonProperty("language")]
                public string Language { get; set; }
                [JsonProperty("language_version")]
                public string LanguageVersion { get; set; }
                [JsonProperty("server")]
                public string Server { get; set; }
                [JsonProperty("server_version")]
                public string ServerVersion { get; set; }
            }

            /// <summary>
            /// 获取一个心跳发送Json数据
            /// </summary>
            /// <param name="localip">本地IP</param>
            public Sender(string localip)
            {
                ClientInfo = new XClientInfo
                {
                    VersionPocs = 0,
                    VersionMain = 3010015,
                    Ip = localip,
                    Language = "csharp",
                    LanguageVersion = ".Net Core 3.1",
                    Server = "none",
                    ServerVersion = "none"
                };
                Cmd = 9001;
            }
        }
        #endregion

        #region Receiver
        public class Receiver : XJsonData.XMsg
        {
            [JsonProperty("vpn_info")]
            public XVpnInfo VpnInfo { get; set; }
            [JsonProperty("sinfo")]
            public XSinfo Sinfo { get; set; }
            public class XVpnInfo {
                [JsonProperty("ports")]
                public string Ports { get; set; }
                [JsonProperty("psk")]
                public string Psk { get; set; }
                [JsonProperty("ups")]
                public XUps Ups { get; set; }
                public class XUps { 
                    [JsonProperty("default_user")]
                    public string DefaultUser { get; set; }
                }
            }
            public class XSinfo { 
                [JsonProperty("switch")]
                public string Switch { get; set; }
                [JsonProperty("mode")]
                public string Mode { get; set; }
                [JsonProperty("transfer")]
                public string Transfer { get; set; } 
                [JsonProperty("dirtyblock")]
                public string DirtyBlock { get; set; }
            }
        }
        #endregion

    }
}

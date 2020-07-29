using Newtonsoft.Json;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class Doki:JsonBase
        {
            #region Sender
            public class Sender : JsonData.XMsg
            {
                [JsonProperty("client_info")]
                public XClientInfo ClientInfo { get; set; }
                [JsonProperty("cmd")]
                public int Cmd { get; set; }

                public class XClientInfo : JsonBase
                {
                    [JsonProperty("version_main")]
                    public int VersionMain { get; set; }
                    [JsonProperty("version_pocs")]
                    public int VersionPocs { get; set; }
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
                    [JsonProperty("req_num")]
                    public int ReqNum { get; set; }
                }

                /// <summary>
                /// 获取一个心跳发送Json数据
                /// </summary>
                /// <param name="localip">本地IP</param>
                public static Sender GetInstance(string localip)
                {
                    return new Sender
                    {
                        ClientInfo = new XClientInfo
                        {
                            VersionMain = 3010016,
                            VersionPocs = 0,
                            Ip = localip,
                            Language = "Java",
                            LanguageVersion = "1.8.0_131",
                            Server = "Apache Tomcat",
                            ServerVersion = "8.5.38",
                            ReqNum = 0
                        },
                        Cmd = 9001,
                    };
                }
            }
            #endregion

            #region Receiver
            public class Receiver : JsonData.XMsg
            {
                [JsonProperty("vpn_info")]
                public XVpnInfo VpnInfo { get; set; }
                [JsonProperty("sinfo")]
                public XSinfo Sinfo { get; set; }
                public class XVpnInfo
                {
                    [JsonProperty("ports")]
                    public string Ports { get; set; }
                    [JsonProperty("psk")]
                    public string Psk { get; set; }
                    [JsonProperty("ups")]
                    public XUps Ups { get; set; }
                    public class XUps
                    {
                        [JsonProperty("default_user")]
                        public string DefaultUser { get; set; }
                    }
                }
                public class XSinfo
                {
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
}

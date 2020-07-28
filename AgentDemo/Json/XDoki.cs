using Newtonsoft.Json;

namespace AgentDemo.Json
{
    public class XDoki : XJsonData.XMsg
    {
        [JsonProperty("client_info")]
        public XClientInfo ClientInfo { get; set; }
        [JsonProperty("cmd")]
        public int Cmd { get; set; }

        public class XClientInfo:XJsonBase
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

        public static XJsonData.XMsg GetInstance(string ip)
        {
            return new XDoki
            {
                ClientInfo = new XClientInfo
                {
                    VersionPocs = 0,
                    VersionMain = 3010015,
                    Ip = ip,
                    Language = "java",
                    LanguageVersion = "1.8.131",
                    Server = "Tomcat",
                    ServerVersion = "8.5.5"
                },
                Cmd = 9001
            };
        }
    }
}

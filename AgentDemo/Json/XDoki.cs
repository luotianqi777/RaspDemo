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

        public XJsonData.XMsg GetInstance()
        {
            return new XDoki
            {
                ClientInfo = new XDoki.XClientInfo
                {
                    VersionPocs = 0,
                    VersionMain = 3000201,
                    Ip = "",
                    Language = "csharp",
                    LanguageVersion = ".net core 3.1",
                    Server = "none",
                    ServerVersion = "none"
                },

            };
        }
    }
}

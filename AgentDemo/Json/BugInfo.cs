using Newtonsoft.Json;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class BugInfo : Msg
        {
            [JsonProperty("result")]
            public XResult Result { get; set; }
            [JsonProperty("cmd")]
            public int Cmd { get; set; }
            public class XResult
            {
                [JsonProperty("vuls")]
                public Vul[] Vuls { get; set; }
                [JsonProperty("tid")]
                public int Tid { get; set; }
            }

            public class Vul
            {
                #region Propertys
                [JsonProperty("vul_iast")]
                public XVulIast VulIast { get; set; }
                [JsonProperty("ustr")]
                public string Ustr { get; set; }
                public class XVulIast
                {
                    [JsonProperty("method")]
                    public string Method { get; set; }
                    [JsonProperty("param")]
                    public string Param { get; set; }
                    [JsonProperty("stacktrace")]
                    public string StackTrace { get; set; }
                    [JsonProperty("type")]
                    public string Type { get; set; }
                    [JsonProperty("info")]
                    public XInfo Info { get; set; }
                    [JsonProperty("url")]
                    public string Url { get; set; }
                    [JsonProperty("httpdata")]
                    public string HttpData { get; set; }
                    public class XInfo
                    {
                        [JsonProperty("name")]
                        public XName Name { get; set; }
                        [JsonProperty("address")]
                        public string Address { get; set; }
                        public class XName
                        {
                            [JsonProperty("$xmiast")]
                            public string Xmiast { get; set; }
                        }
                    }
                }
            #endregion

                public static Vul GetInstance()
                {
                    return new Vul
                    {
                        Ustr = null,
                        VulIast = null,
                    };
                }
            }

            public static BugInfo GetInstance()
            {
                var vuls = new Vul[]{
                    Vul.GetInstance()
                };
                return new BugInfo
                {
                    Cmd = 4001,
                    Result = new XResult
                    {
                        Tid = 1,
                        Vuls = vuls
                    }
                };
            }
        }
    }
}

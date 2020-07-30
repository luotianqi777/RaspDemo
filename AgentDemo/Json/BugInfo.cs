using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

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
                    public string Info { get; set; }
                    [JsonProperty("url")]
                    public string Url { get; set; }
                    [JsonProperty("httpdata")]
                    public string HttpData { get; set; }
                    
                }
            #endregion

                public static Vul GetInstance(HttpRequest request, string info, string stackTrace)
                {
                    return new Vul
                    {
                        Ustr = null,
                        VulIast = new XVulIast
                        {
                            Method = request.Method,
                            Info = info,
                            StackTrace = stackTrace,
                            Url = XTool.HttpHelper.GetUrl(request),
                        }
                    };
                }
            }

            public static BugInfo GetInstance(params Vul[] vuls)
            {
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

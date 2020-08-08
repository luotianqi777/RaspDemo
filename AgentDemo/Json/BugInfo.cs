using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class BugInfo : Msg
        {
            #region Propertys
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
            }
            #endregion

            /// <summary>
            /// 获取漏洞信息json
            /// </summary>
            /// <param name="request">当前请求</param>
            /// <param name="info">漏洞检验info</param>
            /// <param name="stackTrace">函数调用栈</param>
            /// <param name="typeMap">漏洞类型映射：传入类型->返回类型，默认null：返回类型与传入类型相同</param>
            /// <returns></returns>
            public static BugInfo GetInstance(HttpRequest request, string info, string stackTrace,Func<string,string> typeMap=null )
            {
                var url = XTool.HttpHelper.GetUrl(request);
                var headers = request.Headers;
                var jsonString = headers["XMIAST"];
                dynamic json = JsonConvert.DeserializeObject(jsonString);
                var param = request.Method == "GET" ? url[(url.IndexOf('?') + 1)..url.IndexOf('=')] : "";
                string type = json.type;
                if (typeMap != null)
                {
                    type = typeMap(type);
                }
                return new BugInfo
                {
                    Cmd = 4001,
                    Result = new XResult
                    {
                        Tid = json.tid,
                        Vuls = new Vul[] { new Vul{
                            Ustr = json.ustr,
                            VulIast = new Vul.XVulIast {
                                Method = request.Method,
                                Info = info,
                                StackTrace = stackTrace,
                                Url = url,
                                Type = type,
                                Param = param,
                                } 
                            } 
                        }
                    }
                };
            }
        }
    }
}

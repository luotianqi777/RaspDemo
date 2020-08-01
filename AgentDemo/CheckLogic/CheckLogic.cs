using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Web;
using static AgentDemo.Json.XJson;
using static AgentDemo.XTool;

namespace AgentDemo
{
    public partial class CheckLogic
    {

        /// <summary>
        /// 自动检测当前request是否为IAST检测请求，是则检测该位置是否含有漏洞，有则自动发送到IAST服务器。
        /// </summary>
        /// <param name="checker">检测(info)参数是否有攻击内容的方法</param>
        /// <param name="request">当前http请求</param>
        /// <param name="info">hook拿到的info</param>
        /// <param name="stackTrace">函数调用栈</param>
        public static async void Check(Func<string,bool> checker ,HttpRequest request, string info, string stackTrace)
        {
            if (string.IsNullOrEmpty(info)) { return; }
            if (HttpHelper.IsNeedCheck(request))
            {
                var url = HttpUtility.UrlDecode(HttpHelper.GetUrl(request));
                // 获取url中的payload
                var index = url.IndexOf('=');
                if (index == -1) { return; }
                var payload = url.Substring(index + 1);
                // 检测info -> 检测payload -> 检测info是否包含payload
                if (checker(info) && checker(payload) && info.Contains(payload))
                {
                    var msg = BugInfo.GetInstance(request, info, stackTrace);
                    Debuger.WriteLine($"发送的漏洞信息: {msg.GetJsonString()}");
                    await SendJsonMsg(msg, AgentConfig.GetInstance());
                }
            }
        }

        /// <summary>
        /// 自动检测将当前的request是否需要转发，需要的话将包装成检测请求并发送到IAST服务器
        /// </summary>
        /// <param name="request">当前的request</param>
        /// <param name="iastrange">检测范围</param>
        public static async void SendCheckRequest(HttpRequest request, params string[] iastrange)
        {
            // 不带有XMFLOW标记的请求将被转发
            if (!HttpHelper.IsNeedRequest(request))
            {
                try
                {
                    var agent = AgentConfig.GetInstance();
                    var requestJson = Request.GetInstance(request, iastrange);
                    var response = await SendJsonMsg(requestJson, agent);
                    response = GetResponseJsonData(response, agent, out _);
                    Debuger.WriteLine(agent.DEBUG, $"转发的请求: {requestJson.GetJsonString()}");
                    Debuger.WriteLine(agent.DEBUG, $"接收的回复: {JsonConvert.DeserializeObject(response)}");
                }
                catch (Exception e)
                {
                    Debuger.WriteLine($"请求转发失败, 原因: {e.Message}");
                }
            }
        }
    }
}

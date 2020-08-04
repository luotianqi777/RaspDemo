using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Web;
using static AgentDemo.Json.XJson;
using static AgentDemo.XTool;

namespace AgentDemo
{
    public partial class Checker
    {
        public abstract class AbstractChecker
        {

            /// <summary>
            /// 检测info是否有漏洞
            /// </summary>
            /// <param name="info">检测的info</param>
            /// <returns>有则返回true</returns>
            public abstract bool CheckInfo(string info);

            /// <summary>
            /// 检测payload是否有漏洞，默认采用CheckInfoBug
            /// </summary>
            /// <param name="payload">检测的payload</param>
            /// <returns>有则返回true</returns>
            public virtual bool CheckPayload(string payload)
            {
                return CheckInfo(payload);
            }

            /// <summary>
            /// 拦截行为
            /// </summary>
            /// <param name="response">当前回response/param>
            public virtual void BlockAction(HttpResponse response) {
                response.Redirect("/");
            }

        }

        /// <summary>
        /// 自动检测当前request是否为IAST检测请求，是则检测该位置是否含有漏洞，有则自动发送到IAST服务器并根据插件配置选择是否拦截。
        /// </summary>
        /// <param name="checker">检测(info)参数是否有攻击内容的方法</param>
        /// <param name="info">hook拿到的info</param>
        /// <param name="stackTrace">函数调用栈</param>
        public static async void Check(AbstractChecker checker, string info, string stackTrace)
        {
            var context = HttpHelper.GetCurrentHttpContext();
            var request = context?.Request;
            var response = context?.Response;
            if (checker == null || context == null || request == null || response == null || string.IsNullOrEmpty(info)) { return; }
            // 获取url中的payload
            var url = HttpUtility.UrlDecode(HttpHelper.GetUrl(request));
            var index = url.IndexOf('=');
            if (index == -1) { return; }
            var payload = url.Substring(index + 1);
            // 检测info -> 检测payload -> 检测info是否包含payload
            if (checker.CheckInfo(info) && checker.CheckPayload(payload) && info.Contains(payload))
            {
                if (HttpHelper.IsNeedCheck(request))
                {
                    // 发送漏洞信息
                    var msg = BugInfo.GetInstance(request, info, stackTrace);
                    Debuger.WriteLine($"发送的漏洞信息: {msg.GetJsonString()}");
                    await SendJsonMsg(msg);
                }
                // 根据插件配置选择是否拦截
                if (AgentConfig.BLOCK)
                {
                    checker.BlockAction(response);
                }
            }
        }

        /// <summary>
        /// 自动检测将当前的request是否需要转发，需要的话将包装成检测请求并发送到IAST服务器
        /// </summary>
        /// <param name="iastrange">检测范围</param>
        public static async void SendCheckRequest(params string[] iastrange)
        {
            var request = HttpHelper.GetCurrentHttpRequest();
            // 不带有XMFLOW标记的请求将被转发
            if (HttpHelper.IsNeedRequest(request))
            {
                try
                {
                    var requestJson = Request.GetInstance(request, iastrange);
                    var response = await SendJsonMsg(requestJson);
                    Debuger.WriteLine( $"转发的请求: {requestJson.GetJsonString()}");
                    Debuger.WriteLine( $"接收的回复: {JsonConvert.DeserializeObject(response)}");
                }
                catch (Exception e)
                {
                    Debuger.WriteLine($"请求转发失败, 原因: {e.Message}调用栈：{e.StackTrace}");
                }
            }
        }

    }
}

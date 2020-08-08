using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
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
            /// <param name="isPayload">是否是payload</param>
            /// <returns>有则返回true</returns>
            public abstract bool CheckInfo(string info, bool isPayload=false); 

            public virtual string GetPayload(HttpRequest request)
            {
                // 获取url中的payload
                var url = HttpUtility.UrlDecode(HttpHelper.GetUrl(request));
                var index = url.IndexOf('=');
                // 如果为-1则返回整个url
                return url.Substring(index + 1);
            }

            public virtual string PostPayload(HttpRequest request)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 拦截行为
            /// </summary>
            /// <param name="response">当前回response/param>
            public virtual void BlockAction(HttpResponse response) {
                try
                {
                    response.Redirect("/");
                }catch(Exception e)
                {
                    Debuger.WriteLine(e.Message);
                }
            }

        }

        /// <summary>
        /// 检测该位置是否含有漏洞，有则自动检测当前request是否为IAST检测请求，是则自动发送到IAST服务器，然后根据插件配置选择是否拦截。
        /// </summary>
        /// <param name="checker">检测(info)参数是否有攻击内容的方法</param>
        /// <param name="info">hook拿到的info</param>
        /// <param name="stackTrace">函数调用栈</param>
        /// <param name="typeMap">漏洞类型映射：传入类型->返回类型，默认null：返回类型与传入类型相同</param>
        public static async void Check(AbstractChecker checker, string info, string stackTrace, Func<string,string> typeMap = null )
        {
            var context = HttpHelper.GetCurrentHttpContext();
            var request = context?.Request;
            var response = context?.Response;
            if (checker == null || context == null || request == null || response == null || string.IsNullOrEmpty(info)) { return; }
            // 获取payload
            string payload = string.Empty;
            switch (request.Method)
            {
                case "GET":
                    payload = checker.GetPayload(request);
                    break;
                case "POST":
                    payload = checker.PostPayload(request);
                    break;
                default:
                    Debuger.WriteLine("未处理的请求类型");
                    break;
            }
            // 检测info -> 检测payload -> 检测info是否包含payload
            if (checker.CheckInfo(info) && checker.CheckInfo(payload, true) && (info.Contains(payload) || payload.Contains(info)))
            {
                if (HttpHelper.IsNeedCheck(request))
                {
                    // 发送漏洞信息
                    var msg = BugInfo.GetInstance(request, info, stackTrace, typeMap);
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

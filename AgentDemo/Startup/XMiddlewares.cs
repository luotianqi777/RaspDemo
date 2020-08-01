using AgentDemo.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Startup
{
    public class XMiddlewares
    {
        public static RequestDelegate TestMain(RequestDelegate next)
        {
            return async context =>
            {
                Debuger.WriteLine(XTool.HttpHelper.GetUrl(context.Request));

                // 不带有XMFLOW标记的请求将被转发
                if (!XTool.HttpHelper.IsCheckRequest(context.Request))
                {
                    var agent = AgentConfig.GetInstance();
                    try
                    {
                        var requestJson = XJson.Request.GetInstance(context.Request, "sql");
                        var response = await XJson.SendJsonMsg(requestJson, agent);
                        response = XJson.GetResponseJsonData(response, agent, out _);
                        Debuger.WriteLine(agent.DEBUG, $"转发的请求: {requestJson.GetJsonString()}");
                        Debuger.WriteLine(agent.DEBUG, $"接收的回复: {JsonConvert.DeserializeObject(response)}");
                    }
                    catch (Exception e)
                    {
                        Debuger.WriteLine(agent.DEBUG, $"请求转发失败, 原因: {e.Message}");
                    }
                }
                await next(context);
            };
        }
    }
}

using AgentDemo.Json;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Startup
{
    public class XMiddlewares
    {
        public static RequestDelegate HttpMiddle(RequestDelegate next)
        {
            return async context =>
            {
                var agentConfig = AgentConfig.GetInstance();
                XJson.JsonData.XMsg msg = 
                XJson.Doki.Sender.GetInstance(agentConfig.LocalIP);
                // new XJson.Request(context.Request, "sql");
                XJson.JsonData jsonData = new XJson.JsonData(agentConfig.AgentID, msg);
                await XTool.JsonSender.SendJsonData(jsonData, agentConfig)
                .ContinueWith(action => { Debuger.WriteLine(agentConfig.DEBUG, $"响应内容：{action.Result}"); });
                await next(context);
            };
        }
    }
}

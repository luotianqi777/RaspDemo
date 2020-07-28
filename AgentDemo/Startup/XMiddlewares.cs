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
                XJsonData.XMsg msg = 
                new XDoki.Sender(agentConfig.LocalIP);
                // new XRequest(context.Request, "sql");
                XJsonData jsonData = new XJsonData(agentConfig.AgentID, msg);
                await XTool.JsonSender.SendJsonData(jsonData, agentConfig)
                .ContinueWith(action => { Debuger.WriteLine(agentConfig.DEBUG, $"响应内容：{action.Result}"); });
                await next(context);
            };
        }
    }
}

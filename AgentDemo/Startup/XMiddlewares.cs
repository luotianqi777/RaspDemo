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
                Debuger.WriteLine(agentConfig.DEBUG, $"当前url：{Tool.XHttpHelper.GetUrl(context.Request)}");
                await Tool.XHttpHelper.SendDoki(agentConfig)
                // await Tool.XHttpHelper.RequestForward(agentConfig, context.Request, "sql")
                .ContinueWith(action => { Debuger.WriteLine(agentConfig.DEBUG, $"响应内容：{action.Result}"); });
                await next(context);
            };
        }
    }
}

using AgentDemo.Json;
using Microsoft.AspNetCore.Http;
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
                // var agentConfig = AgentConfig.GetInstance();
                // var msg = XJson.Request.GetInstance(context.Request, "sql");
                // var jsonData = XJson.JsonData.GetInstance(agentConfig.AgentID, msg);
                // var response = await XJson.SendJsonData(jsonData, agentConfig);
                // response = XJson.GetResponse(response, out int size);
                // var jsonDataString = XJson.DncryptJsonData(response, agentConfig);
                // Debuger.WriteLine(msg);
                // Debuger.WriteLine(jsonDataString);
                Debuger.WriteLine(XTool.HttpHelper.GetUrl(context.Request));
                await next(context);
            };
        }
    }
}

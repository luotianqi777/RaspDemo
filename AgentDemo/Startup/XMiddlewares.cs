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
                XJson.Msg msg = 
                XJson.Request.GetInstance(context.Request, "sql");
                XJson.JsonData jsonData = XJson.JsonData.GetInstance(agentConfig.AgentID, msg);
                var response = await XJson.SendJsonData(jsonData, agentConfig);
                response = XTool.TypeConverter.BytesToString(response, out int size);
                Debuger.WriteLine(response);
                var jsonDataString = XJson.DncryptJsonData(response, agentConfig);
                Debuger.WriteLine(jsonDataString);
                await next(context);
            };
        }
    }
}

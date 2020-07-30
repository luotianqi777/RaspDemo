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
                if (!XTool.HttpHelper.IsCheckRequest(context.Request))
                {
                    // 要转发的请求
                    // var agent = AgentConfig.GetInstance();
                    // var requestJson = XJson.Request.GetInstance(context.Request, "sql");
                    // var response = await XJson.SendJsonMsg(requestJson, agent);
                    // response = XJson.GetResponseJsonData(response, agent, out _);
                    // Debuger.WriteLine(requestJson);
                    // Debuger.WriteLine(JsonConvert.DeserializeObject(response));
                }
                else
                {
                }
                await next(context);
            };
        }
    }
}

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
                // TODO:
                // XJson.JsonData.XMsg msg = 
                // new XJson.Doki.Sender(agentConfig.LocalIP);
                // // new XJson.Request(context.Request, "sql");
                // XJson.JsonData jsonData = new XJson.JsonData(agentConfig.AgentID, msg);
                // await XTool.JsonSender.SendJsonData(jsonData, agentConfig)
                // .ContinueWith(action => { Debuger.WriteLine(agentConfig.DEBUG, $"响应内容：{action.Result}"); });
                string json = "{\"client_info\":{\"version_main\":3010016,\"version_pocs\":0,\"ip\":\"192.168.91.1\",\"language\":\"Java\",\"language_version\":\"1.8.0_131\",\"server\":\"Apache Tomcat\",\"server_version\":\"8.5.38\",\"req_num\":0},\"cmd\":9001}";
                XJson.Doki.Sender jsonDoki = XJson.GetJson<XJson.Doki.Sender>(json);
                XJson.JsonData jsonData = new XJson.JsonData(agentConfig.AgentID, jsonDoki);
                await XTool.JsonSender.SendJsonData(jsonData, agentConfig);
                await next(context);
            };
        }
    }
}

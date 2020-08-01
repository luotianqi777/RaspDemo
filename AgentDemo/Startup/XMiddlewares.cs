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

                await next(context);
            };
        }
    }
}

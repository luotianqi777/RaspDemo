/* ==============================================================================
* 功能描述：HttpTool  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/21 11:44:46
* ==============================================================================*/
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo
{
    public partial class Tool
    {
        public partial class Http
        {

            /// <summary>
            /// 获取请求的完整url
            /// </summary>
            /// <param name="request">request请求</param>
            /// <returns>请求的url</returns>
            public static string GetUrl(HttpRequest request)
            {
                return new StringBuilder()
                    .Append(request.Scheme)
                    .Append("://")
                    .Append(request.Host)
                    .Append(request.PathBase)
                    .Append(request.Path)
                    .Append(request.QueryString)
                    .ToString();
            }

        }
    }
}

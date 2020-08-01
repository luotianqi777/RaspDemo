using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public abstract class JsonBase
        {
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            /// <summary>
            /// 转换为Json格式(便于观看的那种)字符串, 不能用于反格式化字符串
            /// </summary>
            /// <returns>格式化后的Json字符串</returns>
            public string GetJsonString()
            {
                var jsonString = JsonConvert.DeserializeObject(ToString()).ToString();
                return jsonString;
            }

        }
    }
}

using Newtonsoft.Json;
using System;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class JsonData : JsonBase
        {
            [JsonProperty("id")]
            public string ID { get; set; }
            [JsonProperty("ts")]
            public long TimeStamp { get; set; }
            [JsonProperty("msg")]
            public Msg Msg { get; set; }

            /// <summary>
            /// 返回一个JsonData实例
            /// </summary>
            /// <param name="agentConfig">Agent配置信息</param>
            /// <param name="msg">要发送的json数据</param>
            /// <returns>JsonData实例</returns>
            public static JsonData GetInstance(AgentConfig agentConfig, Msg msg)
            {
                return new JsonData
                {
                    ID = agentConfig.AgentID,
                    TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    Msg = msg,
                };
            }

        }
    }
}

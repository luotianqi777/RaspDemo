using Newtonsoft.Json;
using System;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class JsonData : JsonBase
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("ts")]
            public long TimeStamp { get; set; }
            [JsonProperty("msg")]
            public XMsg Msg { get; set; }

            public abstract class XMsg : JsonBase { }

            /// <summary>
            /// 构造一个JsonData
            /// </summary>
            /// <param name="agentID">AgentID</param>
            /// <param name="msg">要发送的json数据</param>
            public JsonData(string agentID, XMsg msg)
            {
                Id = agentID;
                // TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                TimeStamp = 1595935997;
                Msg = msg;
            }

        }
    }
}

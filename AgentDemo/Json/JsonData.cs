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
            /// 构造一个JsonData
            /// </summary>
            /// <param name="agentID">AgentID</param>
            /// <param name="msg">要发送的json数据</param>
            public static JsonData GetInstance(string agentID, Msg msg)
            {
                return new JsonData
                {
                    ID = agentID,
                    TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    Msg = msg,
                };
            }

            /// <summary>
            /// 获取JsonData
            /// </summary>
            /// <typeparam name="T">Msg类型</typeparam>
            /// <param name="jsonString">json字符串</param>
            /// <returns>JsonData</returns>
            public static JsonData GetJsonData<T>(string jsonString)where T:Msg
            {
                var dataNoMsg = GetJson<JsonDataNoMsg>(jsonString);
                var msg = GetJson<Msg>(jsonString);
                return new JsonData
                {
                    ID = dataNoMsg.ID,
                    TimeStamp = dataNoMsg.TimeStamp,
                    Msg = msg
                };
            }

        }
    }
}

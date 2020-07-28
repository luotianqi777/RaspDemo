using Newtonsoft.Json;
using System;

namespace AgentDemo.Json
{
    public abstract class XJsonBase
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static T GetJson<T>(string jsonString) where T:XJsonBase
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }

    public class XJsonData:XJsonBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("ts")]
        public long TimeStamp { get; set; }
        [JsonProperty("msg")]
        public XMsg Msg { get; set; }

        public abstract class XMsg:XJsonBase { }

        /// <summary>
        /// 构造一个JsonData
        /// </summary>
        /// <param name="agentID">AgentID</param>
        /// <param name="msg">要发送的json数据</param>
        public XJsonData(string agentID, XMsg msg)
        {
            Id = agentID;
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            Msg = msg;
        }

    }
}

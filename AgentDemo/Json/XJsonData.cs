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

        public static XJsonData GetXJsonData(string id, XMsg msg)
        {
            return new XJsonData
            {
                Id = id,
                TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Msg=msg
            };
        }

    }
}

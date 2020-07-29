using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public abstract class Msg : JsonBase { }

        public class JsonDataNoMsg : JsonBase
        {
            [JsonProperty("id")]
            public string ID { get; set; }
            [JsonProperty("ts")]
            public long TimeStamp { get; set; }
        }

    }

}

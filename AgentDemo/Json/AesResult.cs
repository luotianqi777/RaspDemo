using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class AesResult : JsonBase
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("aes")]
            public string Aes { get; set; }
            [JsonProperty("aestag")]
            public string AesTag { get; set; }
            [JsonProperty("aesnonce")]
            public string AesNonce { get; set; }
        }
    }
}

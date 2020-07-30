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

        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentDemo.Json
{
    public abstract class XJsonBase
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

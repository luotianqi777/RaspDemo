using AgentDemo.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Text;

namespace AgentDemo
{
    /// <summary>
    /// 用于加载配置文件
    /// </summary>
    class ConfigureManger
    {
        private static readonly string fileName = "agentConfig.json";
        public static bool ReadFromFile()
        {
            try
            {
                StreamReader stream = new StreamReader(File.OpenRead(fileName));
                var configure = JsonConvert.DeserializeObject<XJson.AgentJson>(stream.ReadToEnd());
                stream.Close();
                AgentConfig.TimeOut = configure.TimeOut;
                AgentConfig.DEBUG = configure.DEBUG;
                AgentConfig.BLOCK = configure.BLOCK;
                AgentConfig.AgentKey = configure.AgentKey;
                AgentConfig.LocalIP = configure.LocalIP;
                return true;
            }
            catch (Exception e)
            {
                Debuger.WriteLine($"加载插件配置失败:{e.Message}");
                return false;
            }
        }

        public static void SaveToFile()
        {
            try
            {
                if (!File.Exists(fileName)) { File.Create(fileName).Close(); }
                StreamWriter stream = new StreamWriter(File.OpenWrite(fileName));
                stream.Write(JsonConvert.DeserializeObject(XJson.AgentJson.GetInstance().ToString()).ToString());
                stream.Flush();
                stream.Close();
            }
            catch (Exception e)
            {
                Debuger.WriteLine($"保存插件配置失败:{e.Message}");
            }
        }
    }
}

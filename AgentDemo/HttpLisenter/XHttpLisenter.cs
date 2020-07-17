/* ==============================================================================
* 功能描述：HttpLisenter  
* 创 建 者：Luo Tian Qi
* 创建日期：2020/7/17 9:51:13
* ==============================================================================*/
using System.Net;

namespace AgentDemo.HttpLisenter
{
    public class XHttpLisenter
    {
        private readonly HttpListener listener = new HttpListener();

        public void Add(string url) => listener.Prefixes.Add(url);
        public void Add(string ip, int port) => Add($"http://{ip}:{port}/");

        public async void Start()
        {
            listener.Start();
            while (listener.IsListening)
            {
                var context = await listener.GetContextAsync();
                Debuger.WriteLine($"Listening {context.Request.Url}");
            }
        }
    }
}

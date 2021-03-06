﻿using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;

namespace AgentDemo.Json
{
    public partial class XJson
    {
        public class Doki:JsonBase
        {
            #region Sender
            public class Sender : Msg
            {
                [JsonProperty("client_info")]
                public XClientInfo ClientInfo { get; set; }
                [JsonProperty("cmd")]
                public int Cmd { get; set; }

                public class XClientInfo : JsonBase
                {
                    [JsonProperty("version_main")]
                    public int VersionMain { get; set; }
                    [JsonProperty("version_pocs")]
                    public int VersionPocs { get; set; }
                    private string localip= string.Empty;
                    [JsonProperty("ip")]
                    public string IP
                    {
                        get
                        {
                            if (!string.IsNullOrEmpty(localip))
                            {
                                return localip;
                            }
                            // 获取本机IP
                            foreach(var address in Dns.GetHostAddresses(Dns.GetHostName()))
                            {
                                if (address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    localip = address.ToString();
                                    break;
                                }
                            }
                            if (string.IsNullOrEmpty(localip))
                            {
                                throw new Exception("找不到本地ipv4");
                            }
                            return localip;
                        }
                    }
                    [JsonProperty("language")]
                    public string Language { get; set; }
                    [JsonProperty("language_version")]
                    public string LanguageVersion { get; set; }
                    [JsonProperty("server")]
                    public string Server { get; set; }
                    [JsonProperty("server_version")]
                    public string ServerVersion { get; set; }
                }

                /// <summary>
                /// 获取一个心跳发送实例
                /// </summary>
                /// <returns>心跳发送实例</returns>
                public static Sender GetInstance()
                {
                    return new Sender
                    {
                        ClientInfo = new XClientInfo
                        {
                            VersionMain = 3010012,
                            VersionPocs = 0,
                            Language = "C#",
                            LanguageVersion = "core 3.1",
                            Server = "",
                            ServerVersion = "",
                        },
                        Cmd = 9001,
                    };
                }
            }
            #endregion

            #region Receiver
            public class Receiver : Msg
            {
                [JsonProperty("vpn_info")]
                public XVpnInfo VpnInfo { get; set; }
                [JsonProperty("sinfo")]
                public XSinfo Sinfo { get; set; }
                public class XVpnInfo
                {
                    [JsonProperty("ports")]
                    public string Ports { get; set; }
                    [JsonProperty("psk")]
                    public string Psk { get; set; }
                    [JsonProperty("ups")]
                    public XUps Ups { get; set; }
                    public class XUps
                    {
                        [JsonProperty("default_user")]
                        public string DefaultUser { get; set; }
                    }
                }
                public class XSinfo
                {
                    [JsonProperty("switch")]
                    public string Switch { get; set; }
                    [JsonProperty("mode")]
                    public string Mode { get; set; }
                    [JsonProperty("transfer")]
                    public string Transfer { get; set; }
                    [JsonProperty("dirtyblock")]
                    public string DirtyBlock { get; set; }
                }
            }
            #endregion

        }
    }
}

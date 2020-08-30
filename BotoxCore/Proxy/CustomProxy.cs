using BotoxNetwork.Server;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Proxy
{
    public class CustomProxy : BaseServer<CustomClient>
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public List<ProxyElement> Elements { get; set; }
        public int ProcessId { get; set; }

        public CustomProxy(int port, int processId) : base(port)
        {
            ProcessId = processId;
            Elements = new List<ProxyElement>();

            OnClientConnected += CustomProxy_OnClientConnected;
            OnClientDisconnected += CustomProxy_OnClientDisconnected;
        }

        private void CustomProxy_OnClientDisconnected(CustomClient obj)
        {
            if(Elements.FirstOrDefault(x => x.LocalClient == obj) is ProxyElement element)
            {
                Elements.Remove(element);
            }
        }

        private void CustomProxy_OnClientConnected(CustomClient obj)
        {
            if(Elements.FirstOrDefault(x => x.LocalClient is null) is ProxyElement proxy)
            {
                proxy.LocalClient = obj;
                proxy.Init();
            }
        }

        public void ConnectRemoteClient(IPEndPoint ip)
        {
            ProxyElement proxy = new ProxyElement()
            {
                LocalClient = null,
                RemoteClient = new CustomClient(ip)
            };

            Elements.Add(proxy);
        }
    }
}

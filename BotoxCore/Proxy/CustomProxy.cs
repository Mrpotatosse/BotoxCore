using BotoxDofusProtocol.Protocol;
using BotoxNetwork.Server;
using BotoxSharedProtocol.Network;
using BotoxSharedProtocol.Network.Interfaces;
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
    public class CustomProxy<T> : BaseServer<CustomClient> where T : ProtocolTreatment
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
            T client_t = (T)Activator.CreateInstance(typeof(T), new object[] { true });
            T server_t = (T)Activator.CreateInstance(typeof(T), new object[] { false });

            ProxyElement proxy = new ProxyElement()
            {
                LocalClient = null,
                RemoteClient = new CustomClient(ip),

                ClientTreatment = client_t,
                ServerTreatment = server_t
            };

            Elements.Add(proxy);
        }
    }
}

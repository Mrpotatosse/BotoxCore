using BotoxCore.Extensions;
using BotoxDofusProtocol.Protocol;
using BotoxNetwork.Server;
using BotoxSharedProtocol.IO.Interfaces;
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

        public List<ProxyElement<T>> Elements { get; set; }
        public int ProcessId { get; set; }

        public uint FAKE_MESSAGE_SENT { get; set; } = 0;
        public uint LAST_GLOBAL_INSTANCE_ID { get; set; } = 0;
        public uint SERVER_MESSAGE_RCV { get; set; } = 0;

        public uint FAKE_MSG_INSTANCE_ID => FAKE_MESSAGE_SENT + LAST_GLOBAL_INSTANCE_ID + SERVER_MESSAGE_RCV;

        public CustomProxy(int port, int processId) : base(port)
        {
            ProcessId = processId;
            Elements = new List<ProxyElement<T>>();

            OnClientConnected += CustomProxy_OnClientConnected;
            OnClientDisconnected += CustomProxy_OnClientDisconnected;
        }

        private void CustomProxy_OnClientDisconnected(CustomClient obj)
        {
            if(Elements.FirstOrDefault(x => x.LocalClient == obj) is ProxyElement<T> element)
            {
                Elements.Remove(element);
            }
        }

        private void CustomProxy_OnClientConnected(CustomClient obj)
        {
            if(Elements.FirstOrDefault(x => x.LocalClient is null) is ProxyElement<T> proxy)
            {
                proxy.LocalClient = obj;
                proxy.Init();
            }
        }

        public void ConnectRemoteClient(IPEndPoint ip)
        {
            T client_t = (T)Activator.CreateInstance(typeof(T), new object[] { true });
            T server_t = (T)Activator.CreateInstance(typeof(T), new object[] { false });

            client_t.FromByteHandler += ProtocolTreatmentExtension.FromBytes;
            server_t.FromByteHandler += ProtocolTreatmentExtension.FromBytes;

            client_t.FromContentHandler += ProtocolTreatmentExtension.FromContent;
            server_t.FromContentHandler += ProtocolTreatmentExtension.FromContent;

            client_t.OnMessageParsed += Client_t_OnMessageParsed;
            server_t.OnMessageParsed += Server_t_OnMessageParsed;

            ProxyElement<T> proxy = new ProxyElement<T>()
            {
                LocalClient = null,
                RemoteClient = new CustomClient(ip),

                ClientTreatment = client_t,
                ServerTreatment = server_t
            };

            Elements.Add(proxy);
        }

        private void Server_t_OnMessageParsed(NetworkElement arg1, ProtocolJsonContent arg2)
        {
            SERVER_MESSAGE_RCV++;
        }

        private void Client_t_OnMessageParsed(NetworkElement arg1, ProtocolJsonContent arg2)
        {
            SERVER_MESSAGE_RCV = 0;
        }
    }
}

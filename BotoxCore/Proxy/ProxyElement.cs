using BotoxNetwork.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Proxy
{
    public class ProxyElement
    {
        public CustomClient LocalClient { get; set; }
        public CustomClient RemoteClient { get; set; }

        public void Init()
        {
            LocalClient.OnClientReceivedData += LocalClient_OnClientReceivedData;
            RemoteClient.OnClientReceivedData += RemoteClient_OnClientReceivedData;

            LocalClient.OnClientDisconnected += LocalClient_OnClientDisconnected;
            RemoteClient.OnClientDisconnected += RemoteClient_OnClientDisconnected;

            RemoteClient.Start();
        }

        private void RemoteClient_OnClientDisconnected(BaseClient obj)
        {
            LocalClient.Stop();
        }

        private void LocalClient_OnClientDisconnected(BaseClient obj)
        {
            RemoteClient.Stop();
        }

        private void RemoteClient_OnClientReceivedData(MemoryStream obj)
        {
            LocalClient.Send(obj.ToArray());
        }

        private void LocalClient_OnClientReceivedData(MemoryStream obj)
        {
            RemoteClient.Send(obj.ToArray());
        }
    }
}

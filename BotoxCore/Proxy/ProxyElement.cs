using BotoxCore.Configurations.Customs;
using BotoxNetwork.Client;
using BotoxSharedProtocol.Network;
using BotoxSharedProtocol.Network.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Proxy
{
    public class ProxyElement
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CustomClient LocalClient { get; set; }
        public CustomClient RemoteClient { get; set; }

        public IProtocolTreatment ClientTreatment { get; set; }
        public IProtocolTreatment ServerTreatment { get; set; }

        public void Init()
        {
            LocalClient.OnClientReceivedData += LocalClient_OnClientReceivedData;
            RemoteClient.OnClientReceivedData += RemoteClient_OnClientReceivedData;

            LocalClient.OnClientDisconnected += LocalClient_OnClientDisconnected;
            RemoteClient.OnClientDisconnected += RemoteClient_OnClientDisconnected;

            ClientTreatment.OnMessageParsed += ClientTreatment_OnMessageParsed;
            ServerTreatment.OnMessageParsed += ServerTreatment_OnMessageParsed;

            RemoteClient.Start();
        }

        private void ServerTreatment_OnMessageParsed(NetworkElement arg1, ProtocolJsonContent arg2)
        {
            StartupConfiguration configuration = Configurations.ConfigurationManager.Instance.Startup;
            if (configuration.show_message)
            {
                logger.Info($"[server {RemoteClient.remoteIp}] {arg1.BasicString()}");
                if (configuration.show_message_content)
                {
                    logger.Info($"{arg2}");
                }
            }
        }

        private void ClientTreatment_OnMessageParsed(NetworkElement arg1, ProtocolJsonContent arg2)
        {
            StartupConfiguration configuration = Configurations.ConfigurationManager.Instance.Startup;
            if (configuration.show_message)
            {
                logger.Info($"[client {RemoteClient.remoteIp}] {arg1.BasicString()}");
                if (configuration.show_message_content)
                {
                    logger.Info($"{arg2}");
                }
            }
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
            ServerTreatment.InitBuild(obj);
            LocalClient.Send(obj.ToArray());
        }

        private void LocalClient_OnClientReceivedData(MemoryStream obj)
        {
            ClientTreatment.InitBuild(obj);
            RemoteClient.Send(obj.ToArray());
        }
    }
}

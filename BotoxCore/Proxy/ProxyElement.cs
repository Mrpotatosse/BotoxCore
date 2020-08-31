using BotoxCore.Configurations.Customs;
using BotoxCore.Hooks;
using BotoxDofusProtocol.Protocol;
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
    public class ProxyElement<T> where T : ProtocolTreatment
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CustomClient LocalClient { get; set; }
        public CustomClient RemoteClient { get; set; }

        public T ClientTreatment { get; set; }
        public T ServerTreatment { get; set; }

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
            var hooker = HookManager<T>.Instance[LocalClient.localIp.Port];
            if(hooker is null)
            {
                logger.Error("no proxy found");
                return;
            }

            if (ClientTreatment.Informations is MessageBuffer informations)
            {
                hooker.Proxy.LAST_GLOBAL_INSTANCE_ID = informations.InstanceId;
            }

            uint instance_id = hooker.Proxy.LAST_GLOBAL_INSTANCE_ID + hooker.Proxy.FAKE_MESSAGE_SENT;
            StartupConfiguration configuration = Configurations.ConfigurationManager.Instance.Startup;
            if (configuration.show_message)
            {
                logger.Info($"[client {RemoteClient.remoteIp}] {arg1.BasicString()}");
                if (configuration.show_message_content)
                {
                    logger.Info($"{arg2}");
                }
            }

            RemoteClient.Send(ClientTreatment.Informations.ReWriteInstanceId(instance_id));
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
            //RemoteClient.Send(obj.ToArray());
        }
    }
}

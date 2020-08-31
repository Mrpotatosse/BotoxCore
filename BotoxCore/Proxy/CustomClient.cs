using BotoxCore.Configurations.Customs;
using BotoxCore.Extensions;
using BotoxCore.Hooks;
using BotoxCore.Protocol;
using BotoxDofusProtocol.IO;
using BotoxDofusProtocol.Protocol;
using BotoxNetwork.Client;
using BotoxSharedProtocol.IO.Interfaces;
using BotoxSharedProtocol.Network;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Proxy
{
    public class CustomClient : BaseClient
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public event Action<NetworkElement, ProtocolJsonContent> OnCustomMessageSent;
        public event Func<uint> GetCustomInstanceId; 

        public CustomClient(IPEndPoint distantIp) : base(distantIp)
        {

        }

        public CustomClient(Socket socket) : base(socket)
        {

        }

        public void Send(string protocolName, ProtocolJsonContent content, bool clientSide = true)
        {
            NetworkElement message = ProtocolManager.Instance.Protocol[ProtocolKeyEnum.Messages, x => x.name == protocolName];
            Send(message, content, clientSide);
        }

        public void Send(int protocolId, ProtocolJsonContent content, bool clientSide = true)
        {
            NetworkElement message = ProtocolManager.Instance.Protocol[ProtocolKeyEnum.Messages, x => x.protocolID == protocolId];
            Send(message, content, clientSide);
        }

        public void Send(NetworkElement message, ProtocolJsonContent content, bool clientSide)
        {
            if (message is null) return;

            using (BigEndianWriter writer = new BigEndianWriter())
            {
                byte[] data = ProtocolTreatmentExtension.FromContent(content, message); 

                int cmpLen = _cmpLen(data.Length);
                writer.WriteShort((short)((message.protocolID << 2) | cmpLen));
                
                if (clientSide)
                {
                    writer.WriteUnsignedInt(GetCustomInstanceId());
                }

                switch (cmpLen)
                {
                    case 0:
                        break;
                    case 1:
                        writer.WriteByte((byte)data.Length);
                        break;
                    case 2:
                        writer.WriteShort((short)data.Length);
                        break;
                    case 3:
                        writer.WriteByte((byte)((data.Length >> 16) & 255));
                        writer.WriteShort((short)(data.Length & 65535));
                        break;
                }

                writer.WriteBytes(data);
                Send(writer.Data);

                OnCustomMessageSent?.Invoke(message, content);

                StartupConfiguration configuration = Configurations.ConfigurationManager.Instance.Startup;
                if (configuration.show_fake_message)
                {
                    logger.Info($"fake message sent to {remoteIp} |{message.BasicString()}]");
                    if (configuration.show_fake_message_content)
                    {
                        logger.Info($"{content}");
                    }
                }
            }
        }

        private int _cmpLen(int length)
        {
            if (length > 65535) return 3;
            if (length > 255) return 2;
            if (length > 0) return 1;
            return 0;
        }
    }
}

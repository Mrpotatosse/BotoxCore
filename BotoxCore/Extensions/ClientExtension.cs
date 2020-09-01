using BotoxCore.Protocol;
using BotoxCore.Proxy;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Extensions
{
    public static class ClientExtension
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// ONLY MESSAGE FOR SERVER !!!
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="content"></param>
        public static void SendChatMessage(this CustomClient client, byte channel, string content)
        {
            client.Send(861, new BotoxSharedProtocol.Network.ProtocolJsonContent()
            {
                fields =
                {
                    { "channel", channel },
                    { "content", content }
                }
            });
        }

        /// <summary>
        /// ONLY MESSAGE FOR SERVER
        /// </summary>
        /// <param name="client"></param>
        /// <param name="channel"></param>
        /// <param name="content"></param>
        public static void SendChatMessage(this CustomClient client, string channelName, string content)
        {
            if (byte.TryParse(ProtocolManager.Instance.Protocol.enumerations.FirstOrDefault(x => x.name == "ChatActivableChannelsEnum")[channelName], out byte channel))
            {
                client.SendChatMessage(channel, content);
            }
            else
            {
                logger.Error($"Send chat error : no channel '{channelName}' found");
            }
        }

        /// <summary>
        /// ONLY MESSAGE FOR SERVER
        /// </summary>
        /// <param name="client"></param>
        /// <param name="receiver"></param>
        /// <param name="content"></param>
        public static void SendPrivateMessage(this CustomClient client, string receiver, string content)
        {
            client.Send(851, new BotoxSharedProtocol.Network.ProtocolJsonContent()
            {
                fields =
                {
                    { "receiver", receiver },
                    { "content", content }
                }
            });
        }
    }
}

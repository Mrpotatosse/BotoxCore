using BotoxCore.Handlers.Interfaces;
using BotoxCore.Protocol;
using BotoxCore.Proxy;
using BotoxDofusProtocol.Protocol;
using BotoxSharedProtocol.Network;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Handlers
{
    public abstract class MessageHandler : IMessageHandler
    {
        protected abstract Logger logger { get; } 

        public abstract void Handle();
        public virtual void EndHandle() { }
        public virtual void Error(Exception e) { }

        private NetworkElement _message { get; set; } = null;
        protected NetworkElement Message
        {
            get
            {
                if(_message is null)
                {
                    HandlerAttribute attribute = GetType().GetCustomAttribute<HandlerAttribute>();

                    if (attribute is null) return null;

                    if (attribute.ProtocolId > 0)
                        _message = ProtocolManager.Instance.Protocol[ProtocolKeyEnum.Messages, x => x.protocolID == attribute.ProtocolId];
                    else if (attribute.ProtocolName != null)
                        _message = ProtocolManager.Instance.Protocol[ProtocolKeyEnum.Messages, x => x.name == attribute.ProtocolName];
                }
                return _message;
            }
        }

        protected ProtocolJsonContent Content { get; set; }
        protected CustomClient LocalClient { get; set; }
        protected CustomClient RemoteClient { get; set; }

        public MessageHandler(CustomClient localClient, CustomClient remoteClient, ProtocolJsonContent content)
        {
            Content = content;
            LocalClient = localClient;
            RemoteClient = remoteClient;
        }
    }
}

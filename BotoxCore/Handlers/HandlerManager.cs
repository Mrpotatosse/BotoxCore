using BotoxCore.Extensions;
using BotoxCore.Handlers.Interfaces;
using BotoxCore.Proxy;
using BotoxSharedProtocol.Network;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Handlers
{
    public class HandlerManager  : Singleton<HandlerManager>
    {
        private IEnumerable<Type> handlersType { get; set; }

        public void Init()
        {
            handlersType = Assembly.GetExecutingAssembly().GetTypes().Where(type => 
            {
                if (type.GetCustomAttribute<HandlerAttribute>() is null) return false;
                if (type.IsAbstract) return false;
                if (!type.IsSubclassOf(typeof(MessageHandler))) return false;

                return true;
            });
        }

        public void Handle(int protocolId, CustomClient local, CustomClient remote, ProtocolJsonContent content)
        {
            Type type = handlersType.FirstOrDefault(x => x.GetCustomAttribute<HandlerAttribute>().ProtocolId == protocolId);
            Handle(type, local, remote, content);
        }

        public void Handle(string protocolName, CustomClient local, CustomClient remote, ProtocolJsonContent content)
        {
            Type type = handlersType.FirstOrDefault(x => x.GetCustomAttribute<HandlerAttribute>().ProtocolName.ToLower() == protocolName.ToLower());
            Handle(type, local, remote, content);
        }

        public void Handle(Type type, CustomClient local, CustomClient remote, ProtocolJsonContent content)
        {
            if (type is null) return;
            MessageHandler handler = (MessageHandler)Activator.CreateInstance(type, new object[] { local, remote, content });

            new Action(handler.Handle).Run(handler.EndHandle, handler.Error);            
        }
    }
}

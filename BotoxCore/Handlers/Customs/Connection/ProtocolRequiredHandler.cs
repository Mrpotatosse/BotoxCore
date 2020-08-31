using BotoxCore.Proxy;
using BotoxSharedProtocol.Network;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Handlers.Customs.Connection
{
    [Handler(ProtocolId = 1)]
    public class ProtocolRequiredHandler : MessageHandler
    {
        protected override Logger logger => LogManager.GetCurrentClassLogger();

        public ProtocolRequiredHandler(CustomClient local, CustomClient remote, ProtocolJsonContent content) 
            : base(local, remote, content)
        {

        }

        public override void Handle()
        {
            logger.Info("handle protocol required");
        }

        public override void EndHandle()
        {
        
        }

        public override void Error(Exception e)
        {

        }
    }
}

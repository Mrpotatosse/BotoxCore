using BotoxCore.Proxy;
using BotoxSharedProtocol.Network;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Handlers.Customs.Map
{
    [Handler(ProtocolId = 226)]
    public class MapComplementaryInformationsDataMessageHandler : MessageHandler
    {
        protected override Logger logger => LogManager.GetCurrentClassLogger();

        public MapComplementaryInformationsDataMessageHandler(CustomClient local, CustomClient remote, ProtocolJsonContent content) 
            : base(local, remote, content)
        {

        }

        public override void Handle()
        {

        }
    }
}

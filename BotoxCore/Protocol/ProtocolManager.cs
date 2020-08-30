using BotoxCore.Extensions;
using BotoxDofusProtocol.Protocol;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Protocol
{
    public class ProtocolManager : Singleton<ProtocolManager>
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private BotofuProtocol _protocol { get; set; }
        public BotofuProtocol Protocol
        {
            get
            {
                if (_protocol is null)
                {

                    while (!BotofuProtocolManager.Download())
                    {
                        logger.Error($"no protocol file found in location : '{BotofuProtocolManager.PROTOCOL_JSON_LOCATION}'");
                        logger.Info("PRESS ANY KEY TO CHECK AGAIN");

                        Console.ReadLine();
                    }

                    _protocol = BotofuProtocolManager.Protocol;
                }
                return _protocol;
            }
        }

        private bool updated { get; set; } = false;

        public bool Update(bool force = false)
        {
            if (updated && !force) return true;

            try
            {
                BotofuProtocolManager.Download();
                logger.Info("protocol is up-to-date");
                updated = true;
            }
            catch(Exception e)
            {
                logger.Error(e);
            }

            return updated;
        }
    }
}

using BotoxCore.Hooks;
using BotoxCore.Logs;
using BotoxCore.Protocol;
using BotoxCore.Proxy;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore
{
    class Program
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            BotoxLogManager.Instance.Init();

            logger.Info($"Botox Alpha version ({DateTime.Now}) - by MrPot");

            ProtocolManager.Instance.Update();

            HookManager.Instance.CreateHooker().Inject();

            Console.ReadLine();
        }
    }
}

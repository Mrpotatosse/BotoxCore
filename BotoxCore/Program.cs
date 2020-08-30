using BotoxCore.Hooks;
using BotoxCore.Logs;
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
                        
            logger.Info("Hello World !");

            HookManager.Instance.CreateHooker().Inject();

            Console.ReadLine();
        }
    }
}

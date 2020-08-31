﻿using BotoxCore.Configurations;
using BotoxCore.Handlers;
using BotoxCore.Hooks;
using BotoxCore.Logs;
using BotoxCore.Protocol;
using BotoxCore.Proxy;
using BotoxDofusProtocol.Protocol;
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
            HandlerManager.Instance.Init();

            // change MessageInformation with your own ProtocolTreatment if you want to use it with another game
            HookManager<MessageInformation>.Instance.CreateHooker().Inject();

            Console.ReadLine();
        }
    }
}

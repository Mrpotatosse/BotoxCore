using BotoxCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using BotoxCore.Configurations;

namespace BotoxCore.Logs
{
    public class BotoxLogManager : Singleton<BotoxLogManager>
    {
        private readonly LoggingConfiguration configuration = new LoggingConfiguration();
        private readonly FileTarget log_file = new FileTarget("log_file") { FileName = "./log.txt" };
        private readonly ConsoleTarget log_console = new ConsoleTarget("log_console");

        public void Init()
        {
            Console.Title = "BotoxCore - Alpha";

            if(ConfigurationManager.Instance.Startup.show_log)
                configuration.AddRule(LogLevel.Info, LogLevel.Fatal, log_console);
            
            if(ConfigurationManager.Instance.Startup.save_log)
                configuration.AddRule(LogLevel.Debug, LogLevel.Fatal, log_file);

            LogManager.Configuration = configuration;
        }
    }
}

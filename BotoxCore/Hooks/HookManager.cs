using BotoxCore.Configurations;
using BotoxCore.Configurations.Customs;
using BotoxCore.Extensions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Hooks
{
    public class HookManager : Singleton<HookManager>
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<int, Hooker> Hooks { get; set; } = new Dictionary<int, Hooker>();

        public Hooker this[Func<Hooker, bool> predicat]
        {
            get
            {
                return Hooks.FirstOrDefault(x => predicat(x.Value)).Value;
            }
        }

        public Hooker this[int port]
        {
            get
            {
                if (Hooks.ContainsKey(port))
                    return Hooks[port];
                return null;
            }
        }

        private bool IsPortUsed(int port)
        {
            if (Hooks.ContainsKey(port))
            {
                return true;
            }


            var global = IPGlobalProperties.GetIPGlobalProperties();
            var tcp = global.GetActiveTcpListeners();

            return tcp.FirstOrDefault(x => x.Port == port) != null;
        }

        public int AvailablePort
        {
            get
            {
                StartupConfiguration configuration = ConfigurationManager.Instance.Startup;

                if(configuration is null)
                {
                    logger.Error("startup configuration is null");
                    return -1;
                }

                int available = configuration.default_proxy_port;
                while (IsPortUsed(available))
                {
                    available = Math.Max(configuration.default_proxy_port, (available + 1) % short.MaxValue);
                }

                return available;
            }
        }

        public Hooker CreateHooker()
        {
            Hooker hooker = new Hooker(AvailablePort);

            hooker.OnProcessExited += Hooker_OnProcessExited;
            hooker.OnProcessStarted += Hooker_OnProcessStarted;

            Hooks.Add(hooker.ProxyPort, hooker);

            return hooker;
        }

        private void Hooker_OnProcessStarted(Hooker obj)
        {
            obj.Proxy.Start();
            // to do
        }

        private void Hooker_OnProcessExited(Hooker obj)
        {
            obj.Proxy.Stop();

            Hooks.Remove(obj.ProxyPort);
            // to do
        }
    }
}

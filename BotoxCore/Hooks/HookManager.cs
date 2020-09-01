using BotoxCore.AutoLogin;
using BotoxCore.Configurations;
using BotoxCore.Configurations.Customs;
using BotoxCore.Extensions;
using BotoxSharedProtocol.Network;
using BotoxSharedProtocol.Network.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Hooks
{
    public class HookManager<T> : Singleton<HookManager<T>> where T : ProtocolTreatment 
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<int, Hooker<T>> Hooks { get; set; } = new Dictionary<int, Hooker<T>>();

        public Hooker<T> this[Func<Hooker<T>, bool> predicat]
        {
            get
            {
                return Hooks.FirstOrDefault(x => predicat(x.Value)).Value;
            }
        }

        public Hooker<T> this[int port]
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

        public Hooker<T> CreateHooker()
        {
            Hooker<T> hooker = new Hooker<T>(AvailablePort);

            hooker.OnProcessExited += Hooker_OnProcessExited;
            hooker.OnProcessStarted += Hooker_OnProcessStarted;

            Hooks.Add(hooker.ProxyPort, hooker);

            return hooker;
        }

        private void Hooker_OnProcessStarted(Hooker<T> obj)
        {
            Process process = Process.GetProcessById(obj.Proxy.ProcessId);
            obj.Proxy.Start();
            // to do
        }

        private void Hooker_OnProcessExited(Hooker<T> obj)
        {
            obj.Proxy.Stop();

            Hooks.Remove(obj.ProxyPort);
            // to do
        }
    }
}

using BotoxCore.Configurations;
using BotoxCore.Configurations.Customs;
using BotoxCore.Proxy;
using BotoxSharedProtocol.Network;
using BotoxSharedProtocol.Network.Interfaces;
using EasyHook;
using NLog;
using SocketHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Hooks
{
    public class Hooker<T> where T : ProtocolTreatment
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public int ProxyPort { get; private set; }
        public CustomProxy<T> Proxy { get; private set; }

        public event Action<Hooker<T>> OnProcessStarted;
        public event Action<Hooker<T>> OnProcessExited;

        public HookElement _hook;

        public Hooker(int proxyPort)
        {
            ProxyPort = proxyPort;
        }

        public void Inject()
        {
            StartupConfiguration configuration = ConfigurationManager.Instance.Startup;

            if(configuration is null)
            {
                logger.Error("startup configuration is null");
                return;
            }

            int port = ProxyPort;

            _hook = new HookElement();
            {
                _hook.IpcServer = RemoteHooking.IpcCreateServer<BotoxHookInterface<T>>(ref _hook.ChannelName, WellKnownObjectMode.Singleton);
            }

            RemoteHooking.CreateAndInject(
                configuration.dofus_location,
                string.Empty,
                0x00000004,
                InjectionOptions.DoNotRequireStrongName,
                configuration.dll_location,
                configuration.dll_location,
                out _hook.ProcessId,
                _hook.ChannelName,
                port);

            Proxy = new CustomProxy<T>(ProxyPort, _hook.ProcessId);

            Process process = Process.GetProcessById(_hook.ProcessId);
            process.EnableRaisingEvents = true;

            if (process.WaitForInputIdle())
            {
                logger.Info($"process opened : {process.ProcessName} {process.Id}");
                OnProcessStarted?.Invoke(this);
            }

            process.Exited += Process_Exited;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            logger.Info($"process leaved : {_hook.ProcessId}");
            OnProcessExited?.Invoke(this);
        }
    }
}

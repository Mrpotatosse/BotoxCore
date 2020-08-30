using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketHook
{
    public class HookInterface : MarshalByRefObject
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public virtual void NotifyInstalled(string processName) => logger.Info($"Injected on process : {processName}");
        public virtual void Message(string message) => logger.Info($"{message}");
        public virtual void Error(Exception exception) => logger.Error(exception);
        public virtual void IpRedirected(IPEndPoint ip, int processId, int redirectionPort) => logger.Info($"Ip redirected from '{ip}' to '127.0.0.1:{redirectionPort}' (processId:'{processId}')");

        public virtual void Ping() => logger.Info("Socket Hook loaded");
    }
}

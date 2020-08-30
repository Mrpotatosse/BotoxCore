using BotoxCore.Proxy;
using SocketHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Hooks
{
    public class BotoxHookInterface : HookInterface
    {
        public override void IpRedirected(IPEndPoint ip, int processId, int redirectionPort)
        {
            HookManager.Instance[redirectionPort].Proxy.ConnectRemoteClient(ip);
        }
    }
}

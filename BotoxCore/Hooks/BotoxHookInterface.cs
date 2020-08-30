using BotoxCore.Proxy;
using BotoxSharedProtocol.Network;
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
    public class BotoxHookInterface<T> : HookInterface where T : ProtocolTreatment 
    {
        public override void IpRedirected(IPEndPoint ip, int processId, int redirectionPort)
        {
            HookManager<T>.Instance[redirectionPort].Proxy.ConnectRemoteClient(ip);
        }
    }
}

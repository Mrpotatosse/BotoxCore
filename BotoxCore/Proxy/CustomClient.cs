using BotoxNetwork.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BotoxCore.Proxy
{
    public class CustomClient : BaseClient
    {
        public CustomClient(IPEndPoint distantIp) : base(distantIp)
        {

        }

        public CustomClient(Socket socket) : base(socket)
        {

        }
    }
}

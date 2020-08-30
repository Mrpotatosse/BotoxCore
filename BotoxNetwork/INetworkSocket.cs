using BotoxNetwork.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BotoxNetwork
{
    public interface INetworkSocket<T> where T : BaseClient
    {
        event Action<T> OnClientConnected;
        event Action<T> OnClientDisconnected;

        Socket socket { get; }
        IPEndPoint ipResolve { get; }

        IPEndPoint remoteIp { get; }
        IPEndPoint localIp { get; }

        void Start();
        void Stop(bool reuse = false);
        bool isRunning { get; }
    }
}

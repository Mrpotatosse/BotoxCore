using BotoxNetwork.Client;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BotoxNetwork.Server
{
    public class BaseServer<T> : INetworkSocket<T> where T : BaseClient
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Socket socket { get; private set; }
        public IPEndPoint ipResolve { get; private set; }

        public IPEndPoint remoteIp => socket.RemoteEndPoint as IPEndPoint;
        public IPEndPoint localIp => socket.LocalEndPoint as IPEndPoint;

        public bool isRunning { get; private set; }

        public event Action<T> OnClientConnected;
        public event Action<T> OnClientDisconnected;

        public BaseServer(int port)
        {
            ipResolve = new IPEndPoint(IPAddress.Any, port);

            socket = new Socket(ipResolve.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp) 
            {
                ReceiveTimeout = -1,
                SendTimeout = -1,
                NoDelay = true
            };

            isRunning = false;
        }

        public void Start()
        {
            try
            {
                if (isRunning)
                {
                    logger.Error("the server is already running");
                    return;
                }

                socket.Bind(ipResolve);
                socket.Listen(10);

                socket.BeginAccept(_clientConnected, socket);

                logger.Info($"server {localIp} started");

                isRunning = true;
            }
            catch(Exception e)
            {
                logger.Error(e);
                isRunning = false;
            }
        }

        public void Stop(bool reuse = false)
        {
            try
            {
                if (!isRunning)
                {
                    logger.Error("the server is not running");
                    return;
                }

                isRunning = false;

                logger.Info($"server {localIp} stoped");
                socket.Dispose();
            }
            catch(Exception e)
            {
                logger.Error(e);
            }
        }

        private void _clientConnected(IAsyncResult ar)
        {
            if (!isRunning) return;

            socket = (Socket)ar.AsyncState;
            T client = (T)Activator.CreateInstance(typeof(T), socket.EndAccept(ar));

            client.OnClientDisconnected += Client_OnClientDisconnected; 
           
            OnClientConnected?.Invoke(client);

            client.Start();
            socket.BeginAccept(_clientConnected, socket);
        }

        private void Client_OnClientDisconnected(BaseClient obj)
        {
            OnClientDisconnected?.Invoke(obj as T);
        }
    }
}

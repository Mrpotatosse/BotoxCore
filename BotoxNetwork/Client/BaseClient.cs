using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BotoxNetwork.Client
{
    public class BaseClient : INetworkSocket<BaseClient>, INetworkClient
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Socket socket { get; private set; }
        public IPEndPoint ipResolve { get; private set; }

        public IPEndPoint remoteIp => socket.RemoteEndPoint as IPEndPoint;
        public IPEndPoint localIp => socket.LocalEndPoint as IPEndPoint;

        public bool isRunning
        {
            get
            {
                if (socket != null && socket.Connected)
                {
                    try
                    {
                        if (socket.Poll(0, SelectMode.SelectRead))
                        {
                            if (socket.Receive(new byte[1], SocketFlags.Peek) == 0)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        private static readonly int RCV_DATA_LENGTH = 4096;

        private byte[] rcvDataBuffer { get; set; }
        private MemoryStream dataRcvBuffer { get; set; }
        private MemoryStream dataSndBuffer { get; set; }
        private bool disconnectRequest { get; set; }

        public event Action<BaseClient> OnClientConnected;
        public event Action<BaseClient> OnClientDisconnected;

        public event Action<MemoryStream> OnClientReceivedData;
        public event Action<MemoryStream> OnClientSentData;

        public BaseClient(IPEndPoint distantIp) 
            : this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            ipResolve = distantIp;
        }

        public BaseClient(Socket socket)
        {
            this.socket = socket;

            disconnectRequest = false;
        }

        private void BeginReceive()
        {
            rcvDataBuffer = new byte[RCV_DATA_LENGTH];
            socket.BeginReceive(rcvDataBuffer, 0, RCV_DATA_LENGTH, SocketFlags.None, _dataReceived, socket);
        }

        public void Send(byte[] data)
        {
            dataSndBuffer = new MemoryStream(data);
            _sendStream();
        }

        private void _sendStream()
        {
            if(isRunning)
                socket.BeginSend(dataSndBuffer.ToArray(), 0, (int)dataSndBuffer.Length, SocketFlags.None, _dataSent, socket);            
        }

        private void _dataSent(IAsyncResult ar)
        {
            OnClientSentData?.Invoke(dataSndBuffer);
        }

        private void _dataReceived(IAsyncResult ar)
        {
            socket = (Socket)ar.AsyncState;

            int len = socket.EndReceive(ar, out SocketError errorCode);

            if (isRunning && errorCode == SocketError.Success && len > 0)
            {
                dataRcvBuffer = new MemoryStream();
                dataRcvBuffer.Write(rcvDataBuffer, 0, len);

                OnClientReceivedData?.Invoke(dataRcvBuffer);

                BeginReceive();
                return;
            }
            Stop();
        }

        public void Start()
        {
            try
            {
                if (isRunning || socket.Connected)
                {
                    logger.Info($"client {remoteIp} connected");

                    OnClientConnected?.Invoke(this);
                    BeginReceive();
                }
                else
                {
                    if(ipResolve is null)
                    {
                        logger.Error("cannot start socket : no remote ip defined");
                        return;
                    }                              
                    
                    socket.BeginConnect(ipResolve, _clientConnected, socket);
                }
            }
            catch(Exception e)
            {
                logger.Error(e);
            }
        }

        private void _clientConnected(IAsyncResult ar)
        {
            logger.Info($"client {remoteIp} connected");

            OnClientConnected?.Invoke(this);

            BeginReceive();
        }

        public void Stop(bool reuse = false)
        {
            try
            {
                if (disconnectRequest) return;

                disconnectRequest = true;
                socket.Disconnect(reuse);

                logger.Info($"client {localIp} disconnected");
                OnClientDisconnected?.Invoke(this);
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }
    }
}

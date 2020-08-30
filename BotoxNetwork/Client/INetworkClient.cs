using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BotoxNetwork.Client
{
    public interface INetworkClient
    {
        event Action<MemoryStream> OnClientReceivedData;
        event Action<MemoryStream> OnClientSentData;
    }
}

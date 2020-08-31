using BotoxSharedProtocol.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BotoxSharedProtocol.Network.Interfaces
{
    public interface IProtocolTreatment
    {
        event Action<NetworkElement, ProtocolJsonContent> OnMessageParsed;

        event Func<ProtocolJsonContent, NetworkElement, IDataReader, ProtocolJsonContent> FromByteHandler;
        event Func<ProtocolJsonContent, NetworkElement, byte[]> FromContentHandler;
        
        void InitBuild(MemoryStream stream);
    }
}

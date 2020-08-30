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

        ProtocolJsonContent FromByte(NetworkElement element, IDataReader reader, ProtocolJsonContent content = null);
        byte[] FromContent(NetworkElement element, IDataWriter writer, ProtocolJsonContent content);

        void InitBuild(MemoryStream stream);
    }
}

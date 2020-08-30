using BotoxSharedProtocol.IO.Interfaces;
using BotoxSharedProtocol.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BotoxSharedProtocol.Network
{
    public abstract class ProtocolTreatment : IProtocolTreatment
    {
        public abstract event Action<NetworkElement, ProtocolJsonContent> OnMessageParsed;

        public abstract ProtocolJsonContent FromByte(NetworkElement element, ref IDataReader reader, ProtocolJsonContent content = null);
        public abstract byte[] FromContent(NetworkElement element, ref IDataWriter writer, ProtocolJsonContent content);
        public abstract void InitBuild(MemoryStream stream);

        public readonly bool ClientSide;

        public ProtocolTreatment(bool clientSide)
        {
            ClientSide = clientSide;
        }
    }
}

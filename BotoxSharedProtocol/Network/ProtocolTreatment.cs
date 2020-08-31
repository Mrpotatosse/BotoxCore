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

        public event Func<ProtocolJsonContent, NetworkElement, IDataReader, ProtocolJsonContent> FromByteHandler;
        public event Func<ProtocolJsonContent, NetworkElement, byte[]> FromContentHandler;

        public abstract void InitBuild(MemoryStream stream);

        public IMessageBuffer Informations { get; set; }

        public readonly bool ClientSide;

        public ProtocolTreatment(bool clientSide)
        {
            ClientSide = clientSide;
        }

        public ProtocolJsonContent FromByte(ProtocolJsonContent arg1, NetworkElement arg2, IDataReader arg3)
        {
            return FromByteHandler?.Invoke(arg1, arg2, arg3);
        }

        public byte[] FromContent(ProtocolJsonContent arg1, NetworkElement arg2)
        {
            return FromContentHandler?.Invoke(arg1, arg2);
        }
    }
}

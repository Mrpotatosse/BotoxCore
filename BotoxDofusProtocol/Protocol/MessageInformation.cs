using BotoxDofusProtocol.IO;
using BotoxSharedProtocol.IO.Interfaces;
using BotoxSharedProtocol.Network;
using BotoxSharedProtocol.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BotoxDofusProtocol.Protocol
{
    public class MessageInformation : IProtocolTreatment
    {
        public MessageBuffer Informations { get; set; }
        public event Action<NetworkElement, ProtocolJsonContent> OnMessageParsed;

        public readonly bool ClientSide;

        private BigEndianReader Reader { get; set; }

        public MessageInformation(bool clientSide)
        {
            ClientSide = clientSide;
            Reader = new BigEndianReader();
            Informations = new MessageBuffer();
        }

        public void InitBuild(MemoryStream stream)
        {
            if (stream.Length > 0) Reader.Add(stream.ToArray(), 0, (int)stream.Length);

            if(Informations.Build(Reader, ClientSide))
            {
                if(BotofuProtocolManager.Protocol[ProtocolKeyEnum.Messages, x => x.protocolID == Informations.MessageId] is NetworkElement message)
                {
                    BigEndianReader reader = new BigEndianReader(Informations.Data);
                    OnMessageParsed?.Invoke(message, FromByte(message, reader));
                }

                Informations = null;
                Informations = new MessageBuffer();

                MemoryStream remnant = new MemoryStream(Reader.ReadBytes((int)Reader.BytesAvailable));

                Reader.Dispose();
                Reader = new BigEndianReader();

                InitBuild(remnant);                
            }

        }

        public ProtocolJsonContent FromByte(NetworkElement element, IDataReader reader, ProtocolJsonContent content = null)
        {
            return new ProtocolJsonContent();
        }

        public byte[] FromContent(NetworkElement element, IDataWriter writer, ProtocolJsonContent content)
        {
            return new byte[0];
        }
    }
}

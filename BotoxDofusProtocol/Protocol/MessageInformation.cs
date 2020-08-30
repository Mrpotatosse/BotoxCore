using BotoxDofusProtocol.IO;
using BotoxSharedProtocol.IO.Interfaces;
using BotoxSharedProtocol.Network;
using BotoxSharedProtocol.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BotoxDofusProtocol.Protocol
{
    public class MessageInformation : ProtocolTreatment
    {
        public override event Action<NetworkElement, ProtocolJsonContent> OnMessageParsed;

        public MessageBuffer Informations { get; set; }
        
        private BigEndianReader Reader { get; set; }

        public MessageInformation(bool clientSide) : base(clientSide)
        {
            Reader = new BigEndianReader();
            Informations = new MessageBuffer();
        }

        public override void InitBuild(MemoryStream stream)
        {
            if (stream.Length > 0) Reader.Add(stream.ToArray(), 0, (int)stream.Length);

            if(Informations.Build(Reader, ClientSide))
            {
                if (BotofuProtocolManager.Protocol[ProtocolKeyEnum.Messages, x => x.protocolID == Informations.MessageId] is NetworkElement message)
                {
                    IDataReader reader = new BigEndianReader(Informations.Data);
                    ProtocolJsonContent content = FromByte(message, ref reader);
                    reader.Dispose();
                    OnMessageParsed?.Invoke(message, content);
                }

                Informations = null;
                Informations = new MessageBuffer();

                stream.Close();
                stream = new MemoryStream(Reader.ReadBytes((int)Reader.BytesAvailable));

                Reader.Dispose();
                Reader = null;
                Reader = new BigEndianReader();

                InitBuild(stream);                
            }

        }

        private async Task ParseMessageAsync(NetworkElement message, ProtocolJsonContent content)
        {
            await Task.Run(() =>
            {
                OnMessageParsed?.Invoke(message, content);
            });
        }

        public override ProtocolJsonContent FromByte(NetworkElement element, ref IDataReader reader, ProtocolJsonContent content = null)
        {
            return new ProtocolJsonContent();
        }

        public override byte[] FromContent(NetworkElement element, ref IDataWriter writer, ProtocolJsonContent content)
        {
            return new byte[0];
        }
    }
}

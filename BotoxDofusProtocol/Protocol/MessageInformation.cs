using BotoxDofusProtocol.IO;
using BotoxSharedProtocol.IO.Interfaces;
using BotoxSharedProtocol.Network;
using BotoxSharedProtocol.Network.Fields;
using BotoxSharedProtocol.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NLog;

namespace BotoxDofusProtocol.Protocol
{
    public class MessageInformation : ProtocolTreatment
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override event Action<NetworkElement, ProtocolJsonContent> OnMessageParsed;
                
        private BigEndianReader Reader { get; set; }

        public MessageInformation(bool clientSide) : base(clientSide)
        {
            Reader = new BigEndianReader();
            Informations = new MessageBuffer();
        }

        public override void InitBuild(MemoryStream stream)
        {
            MessageBuffer informations = Informations as MessageBuffer;
            if (stream.Length > 0) Reader.Add(stream.ToArray(), 0, (int)stream.Length);

            if(informations.Build(Reader, ClientSide))
            {
                if (BotofuProtocolManager.Protocol[ProtocolKeyEnum.Messages, x => x.protocolID == informations.MessageId] is NetworkElement message)
                {
                    IDataReader reader = new BigEndianReader(informations.Data);
                    ProtocolJsonContent content = FromByte(null, message, reader);
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
    }
}

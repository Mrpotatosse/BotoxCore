using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxDofusProtocol.Protocol
{
    [Flags]
    public enum ProtocolKeyEnum
    {
        None = 0b00,
        Messages = 0b01,
        Types = 0b10,
        MessagesAndTypes = Messages | Types
    }
}

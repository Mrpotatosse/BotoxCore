using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedProtocol.IO.Interfaces
{
    public interface IMessageBuffer
    {
        byte[] ReWriteInstanceId(uint id);
    }
}

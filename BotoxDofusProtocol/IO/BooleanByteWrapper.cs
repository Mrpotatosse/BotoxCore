using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxDofusProtocol.IO
{
    public class BooleanByteWrapper
    {
        public static byte SetFlag(byte flag, byte offset, bool value)
        {
            return value ? (byte)(flag | (1 << offset)) : (byte)(flag & 255 - (1 << offset));
        }

        public static bool GetFlag(byte flag, byte offset)
        {
            return (flag & (byte)(1 << offset)) != 0;
        }
    }
}

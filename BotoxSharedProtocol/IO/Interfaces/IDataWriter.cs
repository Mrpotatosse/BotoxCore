using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedProtocol.IO.Interfaces
{
    public interface IDataWriter
    {
        void WriteByte(byte @byte);
        void WriteUnsignedByte(sbyte @sbyte);

        void WriteShort(short @short);
        void WriteUnsignedShort(ushort @ushort);
        void WriteVarShort(short @short);// custom short
        void WriteVarUhShort(ushort @ushort); // custom ushort

        void WriteInt(int @int);
        void WriteUnsignedInt(uint @uint);
        void WriteVarInt(int @int); // custom int
        void WriteVarUhInt(uint @uint);// custom uint

        void WriteLong(long @long);
        void WriteUnsignedLong(ulong @ulong);
        void WriteVarLong(long @long);// custom long
        void WriteVarUhLong(ulong @ulong); // custom ulong

        void WriteDouble(double @double);
        void WriteFloat(float @float);

        void WriteUTF(string @string);

        void WriteBoolean(bool @bool);
    }
}

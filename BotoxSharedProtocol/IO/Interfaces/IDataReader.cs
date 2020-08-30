using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedProtocol.IO.Interfaces
{
    public interface IDataReader
    {
        byte ReadByte();
        sbyte ReadUnsignedByte();// it should be SignedByte but in dofus it is called unsignedbyte ( rmd : byte[0,255] sbyte[-127,127] )

        short ReadShort();
        ushort ReadUnsignedShort();
        short ReadVarShort();// custom short
        ushort ReadVarUhShort(); // custom ushort

        int ReadInt();
        uint ReadUnsignedInt();
        int ReadVarInt(); // custom int
        uint ReadVarUhInt(); // custom uint

        long ReadLong();
        ulong ReadUnsignedLong();
        long ReadVarLong(); // custom long
        ulong ReadVarUhLong(); // custoom ulong

        double ReadDouble();
        float ReadFloat();

        string ReadUTF();

        bool ReadBoolean();
    }
}

﻿using BotoxDofusProtocol.IO;
using BotoxSharedProtocol.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxDofusProtocol.Protocol
{
    public class MessageBuffer : IMessageBuffer
    {
        public bool IsValid
        {
            get
            {
                return Header.HasValue && Length.HasValue &&
                       Length == Data.Length;
            }
        }

        public int? Header { get; private set; }

        public uint InstanceId { get; private set; }

        public int? MessageId
        {
            get
            {
                if (!Header.HasValue)
                    return null;
                return Header >> 2;
            }
        }

        public int? LengthBytesCount
        {
            get
            {
                if (!Header.HasValue)
                    return null;
                return Header & 0x3;
            }
        }

        public int? Length { get; private set; }

        private byte[] m_data;

        public byte[] Data
        {
            get { return m_data; }
            private set { m_data = value; }
        }

        public byte[] FullPacket { get; set; }

        /// <summary>
        /// ONLY CLIENT SIDE !!!
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public byte[] ReWriteInstanceId(uint instanceId)
        {
            using (BigEndianReader reader = new BigEndianReader(FullPacket))
            {
                using (BigEndianWriter writer = new BigEndianWriter())
                {
                    // header
                    writer.WriteShort(reader.ReadShort());
                    // instance id
                    writer.WriteUnsignedInt(instanceId);
                    // skip
                    reader.ReadUnsignedInt();
                    // len + data
                    writer.WriteBytes(reader.ReadBytes((int)reader.BytesAvailable));

                    return writer.Data;
                }
            }
        }

        public bool Build(BigEndianReader reader, bool clientSide)
        {
            FullPacket = reader.Data;

            if (IsValid)
                return true;

            if (reader.BytesAvailable >= 2 && !Header.HasValue)
            {
                Header = reader.ReadShort();
            }

            if (clientSide)
                InstanceId = reader.ReadUnsignedInt();

            if (LengthBytesCount.HasValue &&
                reader.BytesAvailable >= LengthBytesCount && !Length.HasValue)
            {
                Length = 0;
                for (int i = LengthBytesCount.Value - 1; i >= 0; i--)
                {
                    Length |= reader.ReadByte() << (i * 8);
                }
            }

            if (Data == null && Length.HasValue)
            {
                if (Length == 0)
                    Data = new byte[0];
                if (reader.BytesAvailable >= Length)
                {
                    Data = reader.ReadBytes(Length.Value);
                }
                else if (Length > reader.BytesAvailable)
                {
                    Data = reader.ReadBytes((int)reader.BytesAvailable);
                }
            }
            if (Data != null && Length.HasValue && Data.Length < Length)
            {
                int bytesToRead = 0;
                if (Data.Length + reader.BytesAvailable < Length)
                    bytesToRead = (int)reader.BytesAvailable;
                else if (Data.Length + reader.BytesAvailable >= Length)
                    bytesToRead = Length.Value - Data.Length;

                if (bytesToRead != 0)
                {
                    int oldLength = Data.Length;
                    Array.Resize(ref m_data, (int)(Data.Length + bytesToRead));
                    Array.Copy(reader.ReadBytes(bytesToRead), 0, Data, oldLength, bytesToRead);
                }
            }
            return IsValid;
        }

        ~MessageBuffer()
        {
            Header = null;
            InstanceId = 0;
            Length = null;
            Data = null;
            FullPacket = null;
        }
    }
}

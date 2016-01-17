using Snifles.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Snifles.Application_Layer
{
    public sealed class DnsAnswer
    {
        public bool Cache { get { return TTL != 0; } }

        public readonly string Name;
        public readonly QType Type;
        public readonly DnsClass Class;

        public readonly uint TTL;
        public readonly ushort ByteCount;
        public readonly byte[] RData;

        public DnsAnswer(NetBinaryReader br)
        {
            long currentPos = -1;
            ushort aName = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            if ((aName & 0xC000) == 0xC000) // Pointer
            {
                ushort offset = (ushort)(aName & 0x3FFF);
                currentPos = br.BaseStream.Position;
                br.BaseStream.Position = offset - DnsHeader.BYTE_COUNT;
            }
            else br.BaseStream.Position -= 2;

            List<string> labels = new List<string>();
            byte nameLength;

            while ((nameLength = br.ReadByte()) != 0)
            {
                string label = string.Empty;

                for (int i = 0; i < nameLength; i++)
                {
                    label += (char)br.ReadByte();
                }

                labels.Add(label);
            }

            if (currentPos != -1) br.BaseStream.Position = currentPos;
            Name = string.Join(".", labels);

            Type = (QType)(ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            ushort rawClass = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            if (rawClass > 65279) rawClass = 0;
            else if (rawClass > 4 && rawClass < 252) rawClass = 2;
            else if (rawClass > 255 && rawClass < 65280) rawClass = 2;
            Class = (DnsClass)rawClass;

            TTL = (uint)IPAddress.NetworkToHostOrder(br.ReadInt32());
            ByteCount = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            RData = new byte[ByteCount];
            for (int i = 0; i < ByteCount; i++)
            {
                RData[i] = br.ReadByte();
            }
        }
    }
}

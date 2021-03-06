﻿using Snifles.Data;
using System.Diagnostics;
using System.Net;

namespace Snifles.Application_Layer
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class DnsAnswer
    {
        public bool Cache { get { return TTL != 0; } }

        public readonly string Name;
        public readonly QType Type;
        public readonly DnsClass Class;

        public readonly uint TTL;
        public readonly ushort ByteCount;

        private object recordValue;

        public DnsAnswer(NetBinaryReader nbr)
        {
            Name = nbr.ReadLblOrPntString();
            Type = (QType)nbr.ReadUInt16();

            ushort rawClass = nbr.ReadUInt16();
            if (rawClass > 65279) rawClass = 0;
            else if (rawClass > 4 && rawClass < 252) rawClass = 2;
            else if (rawClass > 255 && rawClass < 65280) rawClass = 2;
            Class = (DnsClass)rawClass;

            TTL = nbr.ReadUInt32();
            ByteCount = nbr.ReadUInt16();
            HandleRData(nbr);
        }

        public override string ToString()
        {
            return $"{Type} From {Name}";
        }

        private void HandleRData(NetBinaryReader nbr)
        {
            long startPos = nbr.BaseStream.Position;

            switch (Type)
            {
                case (QType.A):
                case (QType.AAAA):
                    recordValue = new IPAddress(nbr.ReadBytes(ByteCount));
                    break;
                    /*
                case (QType.NS):
                case (QType.CNAME):
                    recordValue = nbr.ReadLblOrPntString();
                    ReadPadOctets(nbr, startPos);
                    break;
                case (QType.SOA):
                    recordValue = new SOA(nbr);
                    ReadPadOctets(nbr, startPos);
                    break;
                    */
                default:
                    recordValue = nbr.ReadBytes(ByteCount);
                    break;
            }
        }

        private void ReadPadOctets(NetBinaryReader nbr, long startPos)
        {
            long padding = ByteCount - (nbr.BaseStream.Position - startPos);
            nbr.BaseStream.Position += padding;
        }
    }
}
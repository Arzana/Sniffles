using Snifles.Data;
using System;

namespace Snifles.Internet_Layer
{
    public sealed class IcmpHeader : ProtocolHeader
    {
        private const int OCTET_COUNT = 4;

        public readonly MessageType Type;
        public readonly byte Code;

        public readonly byte[] data;

        public IcmpHeader(byte[] byIpData, int start, int bytesReceived)
        {
            NetBinaryReader nbr = new NetBinaryReader(byIpData, start);

            byte type = nbr.ReadByte();
            if (type == 2 || type == 7) type = 1;
            else if (type > 20 && type < 30) type = 20;
            else if (type > 41) type = 41;
            Type = (MessageType)type;

            Code = nbr.ReadByte();
            Checksum = nbr.ReadUInt16();

            data = new byte[bytesReceived - start - OCTET_COUNT];
            Array.Copy(byIpData, start + OCTET_COUNT, data, 0, data.Length);
        }
    }
}
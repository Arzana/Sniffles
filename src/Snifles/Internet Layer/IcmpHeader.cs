using Snifles.Data;
using System;
using System.Net;

namespace Snifles.Internet_Layer
{
    public sealed class IcmpHeader : ProtocolHeader
    {
        public readonly MessageType Type;
        public readonly byte Code;

        public readonly byte[] raw;
        public readonly byte[] data;

        public IcmpHeader(byte[] byIpData, int start, int bytesReceived)
        {
            NetBinaryReader br = new NetBinaryReader(byIpData, start);

            byte type = br.ReadByte();
            if (type == 2 || type == 7) type = 1;
            else if (type > 20 && type < 30) type = 20;
            else if (type > 41) type = 41;
            Type = (MessageType)type;

            Code = br.ReadByte();
            Checksum = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            raw = new byte[4];
            Array.Copy(byIpData, start, raw, 0, raw.Length);

            data = new byte[bytesReceived - start - raw.Length];
            Array.Copy(byIpData, start + raw.Length, data, 0, data.Length);
        }
    }
}
using Snifles.Data;
using System;

namespace Snifles.Transport_Layer
{
    public sealed class UdpHeader : ProtocolHeader
    {
        public const int OCTET_COUNT = 8;
        public bool IsDns { get { return SourcePort == 53 || DestinationPort == 53; } }

        public readonly ushort SourcePort;
        public readonly ushort DestinationPort;

        public readonly ushort ByteCount;
        public readonly byte[] data;

        public UdpHeader(byte[] byIpData, int start, int bytesReceived)
        {
            NetBinaryReader nbr = new NetBinaryReader(byIpData, start);

            SourcePort = nbr.ReadUInt16();
            DestinationPort = nbr.ReadUInt16();

            ByteCount = nbr.ReadUInt16();
            Checksum = nbr.ReadUInt16();

            data = new byte[ByteCount - OCTET_COUNT];
            Array.Copy(byIpData, start + OCTET_COUNT, data, 0, data.Length);
        }
    }
}
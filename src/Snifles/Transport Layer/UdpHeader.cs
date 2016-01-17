using Snifles.Data;
using System;
using System.Net;

namespace Snifles.Transport_Layer
{
    public sealed class UdpHeader : ProtocolHeader
    {
        public bool IsDns { get { return SourcePort == 53 || DestinationPort == 53; } }

        public readonly ushort SourcePort;
        public readonly ushort DestinationPort;

        public readonly ushort ByteCount;

        public readonly byte[] raw;
        public readonly byte[] data;

        public UdpHeader(byte[] byIpData, int start, int bytesReceived)
        {
            NetBinaryReader br = new NetBinaryReader(byIpData, start);

            SourcePort = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            DestinationPort = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            ByteCount = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            Checksum = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            raw = new byte[8];
            Array.Copy(byIpData, start, raw, 0, raw.Length);

            data = new byte[ByteCount - raw.Length];
            Array.Copy(byIpData, start + raw.Length, data, 0, data.Length);
        }
    }
}
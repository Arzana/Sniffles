using Snifles.Application_Layer;
using Snifles.Internet_Layer;
using Snifles.Transport_Layer;
using System.Net;
using System.Net.Sockets;

namespace Snifles
{
    public sealed class Packet
    {
        public ProtocolType Protocol { get { return IpHeader.Protocol; } }
        public IPAddress SenderAddress { get { return IpHeader.Source; } }
        public IPAddress DestinationAddress { get { return IpHeader.Destination; } }

        public readonly IPHeader IpHeader;
        public readonly ProtocolHeader TransportHeader;
        public readonly object ApplicationHeader;

        private readonly byte[] rawData;

        public Packet(byte[] raw, int byteCount)
        {
            rawData = raw;
            IpHeader = new IPHeader(raw, byteCount);

            byte byHeaderLength = (byte)(IpHeader.Version + IpHeader.HeaderLength);
            byHeaderLength <<= 4;
            byHeaderLength >>= 4;
            byHeaderLength *= 4;

            switch (Protocol)
            {
                case (ProtocolType.Udp):
                    UdpHeader udpHeader = new UdpHeader(raw, byHeaderLength, byteCount);
                    TransportHeader = udpHeader;

                    if (udpHeader.IsDns) ApplicationHeader = new DnsHeader(raw, byHeaderLength + udpHeader.raw.Length, byteCount);
                    break;
                case (ProtocolType.Tcp):
                    TcpHeader tcpHeader = new TcpHeader(raw, byHeaderLength);
                    TransportHeader = tcpHeader;
                    break;
                case (ProtocolType.Icmp):
                    IcmpHeader icmpHeader = new IcmpHeader(raw, byHeaderLength, byteCount);
                    TransportHeader = icmpHeader;
                    break;
            }
        }
    }
}

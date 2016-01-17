using Snifles.Data;
using System.Net;

namespace Snifles.Transport_Layer
{
    public sealed class TcpHeader : ProtocolHeader
    {
        public bool IsDns { get { return SourcePort == 53 || DestinationPort == 53; } }

        public readonly ushort SourcePort;
        public readonly ushort DestinationPort;

        public readonly uint SequenceNumber;
        public readonly uint AcknowledgmentNumber;

        public readonly byte DataOffset;

        public readonly bool URG;   // Urgent Pointer field significant.
        public readonly bool ACK;   // Acknowledgment field significant.
        public readonly bool PSH;   // Push Function.
        public readonly bool RST;   // Reset the connection.
        public readonly bool SYN;   // Synchronize sequence numbers.
        public readonly bool FIN;   // No more data from sender.

        public readonly ushort Window;
        public readonly ushort UrgentPointer;

        public TcpHeader(byte[] byIpData, int start)
        {
            NetBinaryReader br = new NetBinaryReader(byIpData, start);

            SourcePort = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            DestinationPort = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            SequenceNumber = br.ReadUInt32();
            AcknowledgmentNumber = br.ReadUInt32();

            DataOffset = br.ReadNible();
            br.SkipPadBits();

            br.ReadCustomAmount(2);
            URG = br.ReadBit();
            ACK = br.ReadBit();
            PSH = br.ReadBit();
            RST = br.ReadBit();
            SYN = br.ReadBit();
            FIN = br.ReadBit();

            Window = br.ReadUInt16();
            Checksum = br.ReadUInt16();
            UrgentPointer = br.ReadUInt16();

            //TODO: finish
        }
    }
}

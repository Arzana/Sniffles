using Snifles.Data;
using System.Net;
using System.Net.Sockets;

namespace Snifles.Internet_Layer
{
    public sealed class IPHeader
    {
        public readonly int Version;
        public readonly int HeaderLength;

        public readonly Precedence Precedence;
        public readonly bool LowDelay;
        public readonly bool HighThroughput;
        public readonly bool HighRelibility;

        public readonly ushort TotalLength;
        public readonly ushort Identification;

        public readonly bool MayFragment;
        public readonly bool LastFragment;
        public readonly int FragmentOffset;

        public readonly byte TTL;
        public readonly ProtocolType Protocol;
        public readonly short HeaderChecksum;

        public readonly IPAddress Source;
        public readonly IPAddress Destination;

        public IPHeader(byte[] byBuffer, int nReceived)
        {
            NetBinaryReader br = new NetBinaryReader(byBuffer, 0, nReceived);

            Version = br.ReadNible();
            HeaderLength = br.ReadNible();

            Precedence = (Precedence)br.ReadCustomAmount(3);
            LowDelay = br.ReadBit();
            HighThroughput = br.ReadBit();
            HighRelibility = br.ReadBit();
            br.SkipPadBits();

            TotalLength = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            Identification = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            br.ReadBit();
            MayFragment = !br.ReadBit();
            LastFragment = !br.ReadBit();
            FragmentOffset = (br.ReadPadBits() << 3) + br.ReadByte();

            TTL = br.ReadByte();
            Protocol = (ProtocolType)br.ReadByte();
            HeaderChecksum = IPAddress.NetworkToHostOrder(br.ReadInt16());

            Source = new IPAddress((uint)br.ReadInt32());
            Destination = new IPAddress((uint)br.ReadInt32());
        }
    }
}
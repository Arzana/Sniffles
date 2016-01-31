using Snifles.Data;
using System.Net;
using System.Net.Sockets;

namespace Snifles.Internet_Layer
{
    public sealed class IPHeader
    {
        public const int OCTET_COUNT = 20;

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
            NetBinaryReader nbr = new NetBinaryReader(byBuffer, 0, nReceived);

            Version = nbr.ReadNible();
            HeaderLength = nbr.ReadNible();

            Precedence = (Precedence)nbr.ReadCustomAmount(3);
            LowDelay = nbr.ReadBit();
            HighThroughput = nbr.ReadBit();
            HighRelibility = nbr.ReadBit();
            nbr.SkipPadBits();

            TotalLength = nbr.ReadUInt16();
            Identification = nbr.ReadUInt16();

            nbr.ReadBit();
            MayFragment = !nbr.ReadBit();
            LastFragment = !nbr.ReadBit();
            FragmentOffset = (nbr.ReadPadBits() << 3) + nbr.ReadByte();

            TTL = nbr.ReadByte();
            Protocol = (ProtocolType)nbr.ReadByte();
            HeaderChecksum = IPAddress.NetworkToHostOrder(nbr.ReadInt16());

            Source = new IPAddress(nbr.ReadBytes(4));
            Destination = new IPAddress(nbr.ReadBytes(4));
        }
    }
}
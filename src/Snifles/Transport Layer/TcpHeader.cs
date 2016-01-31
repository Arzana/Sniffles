using Snifles.Data;
using System;

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
        public readonly byte[][] Options;
        public readonly byte[] Data;

        public TcpHeader(byte[] byIpData, int start)
        {
            NetBinaryReader nbr = new NetBinaryReader(byIpData, start);

            SourcePort = nbr.ReadUInt16();
            DestinationPort = nbr.ReadUInt16();

            SequenceNumber = nbr.ReadUInt32();
            AcknowledgmentNumber = nbr.ReadUInt32();

            DataOffset = nbr.ReadNible();
            nbr.SkipPadBits();

            nbr.ReadCustomAmount(2);
            URG = nbr.ReadBit();
            ACK = nbr.ReadBit();
            PSH = nbr.ReadBit();
            RST = nbr.ReadBit();
            SYN = nbr.ReadBit();
            FIN = nbr.ReadBit();

            Window = nbr.ReadUInt16();
            Checksum = nbr.ReadUInt16();
            UrgentPointer = nbr.ReadUInt16();

            //TODO: test
            byte option;
            long optionsStart = nbr.BaseStream.Position;
            Options = new byte[0][];

            while ((option = nbr.ReadByte()) != 0 || nbr.BaseStream.Position > start + DataOffset)    // 0 = End list
            {
                if (option == 1) continue;  // 1 = No Option
                if (option == 2)            // Max Segm size
                {
                    byte length = nbr.ReadByte();

                    byte[] cur = new byte[length];
                    for (byte i = 0; i < length; i++)
                    {
                        cur[i] = nbr.ReadByte();
                    }

                    Array.Resize(ref Options, Options.Length + 1);
                    Options[Options.Length - 1] = cur;
                }
            }

            long dataLength = DataOffset - optionsStart;
            Data = new byte[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                Data[i] = nbr.ReadByte();
            }
        }
    }
}
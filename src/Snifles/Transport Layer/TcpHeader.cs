using Snifles.Data;
using System;
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
        public readonly byte[][] Options;
        public readonly byte[] Data;

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

            //TODO: test
            byte option;
            long optionsStart = br.BaseStream.Position;
            Options = new byte[0][];

            while ((option = br.ReadByte()) != 0 || br.BaseStream.Position > start + DataOffset)    // 0 = End list
            {
                if (option == 1) continue;  // 1 = No Option
                if (option == 2)            // Max Segm size
                {
                    byte length = br.ReadByte();

                    byte[] cur = new byte[length];
                    for (byte i = 0; i < length; i++)
                    {
                        cur[i] = br.ReadByte();
                    }

                    Array.Resize(ref Options, Options.Length + 1);
                    Options[Options.Length - 1] = cur;
                }
            }

            long dataLength = DataOffset - optionsStart;
            Data = new byte[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                Data[i] = br.ReadByte();
            }
        }
    }
}
using Snifles.Application_Layer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Snifles.Data
{
    public sealed class NetBinaryReader : BinaryReader
    {
        private byte padByte;
        private byte bitPosition;

        public NetBinaryReader(byte[] data)
            : base(new MemoryStream(data, 0, data.Length))
        { }

        public NetBinaryReader(byte[] data, int index)
            : base(new MemoryStream(data, index, data.Length - index))
        { }

        public NetBinaryReader(byte[] data, int index, int length)
            : base(new MemoryStream(data, index, length))
        { }

        new public ushort ReadUInt16()
        {
            return (ushort)IPAddress.NetworkToHostOrder(ReadInt16());
        }

        new public uint ReadUInt32()
        {
            return (uint)IPAddress.NetworkToHostOrder(ReadInt32());
        }

        new public ulong ReadUInt64()
        {
            return (ulong)IPAddress.NetworkToHostOrder(ReadInt64());
        }

        public string ReadLblOrPntString()
        {
            ushort aName = ReadUInt16();

            if (IsPointer(aName)) return GetPointerName(aName);
            else
            {
                BaseStream.Position -= 2;

                List<string> labels = new List<string>();
                byte nameLength;

                while ((nameLength = ReadByte()) != 0)
                {
                    BaseStream.Position -= 1;
                    aName = ReadUInt16();

                    if (IsPointer(aName))
                    {
                        labels.Add(GetPointerName(aName));
                        BaseStream.Position++;
                        break;
                    }

                    BaseStream.Position -= 1;
                    string label = string.Empty;

                    for (int i = 0; i < nameLength; i++)
                    {
                        label += (char)ReadByte();
                    }

                    labels.Add(label);
                }

                return string.Join(".", labels);
            }
        }

        public bool ReadBit()
        {
            if (bitPosition == 0) padByte = ReadByte();

            byte mask = GetBitMask();
            bitPosition += (byte)(bitPosition + 1 >= 8 ? -bitPosition : 1);
            return Convert.ToBoolean(padByte & mask);
        }

        public byte ReadNible()
        {
            if (bitPosition + 4 > 8) SkipPadBits();
            if (bitPosition == 0) padByte = ReadByte();

            byte mask = GetNibleMask();
            bitPosition += (byte)(bitPosition + 4 >= 8 ? -bitPosition : 4);
            return (byte)(padByte & mask);
        }

        public byte ReadCustomAmount(int count)
        {
            if (count > 8) throw new ArgumentException();
            if (bitPosition + count > 8) SkipPadBits();
            if (bitPosition == 0) padByte = ReadByte();

            byte mask = GetCustomMask(count);
            bitPosition += (byte)(bitPosition + count >= 8 ? -bitPosition : count);
            return (byte)(padByte & mask);
        }

        public byte ReadPadBits()
        {
            byte result = ReadCustomAmount(8 - bitPosition);
            SkipPadBits();
            return result;
        }

        public void SkipPadBits()
        {
            padByte = 0;
            bitPosition = 0;
        }

        private string GetPointerName(ushort pointer)
        {
            ushort offset = (ushort)(pointer & 0x3FFF);
            long currentPos = BaseStream.Position;
            BaseStream.Position = offset - DnsHeader.OCTET_COUNT;

            string name = ReadLblOrPntString();
            BaseStream.Position = currentPos;
            return name;
        }

        private bool IsPointer(ushort aName)
        {
            return (aName & 0xC000) == 0xC000;
        }

        private byte GetBitMask()
        {
            switch (bitPosition)
            {
                case (0): return 0x80;
                case (1): return 0x40;
                case (2): return 0x20;
                case (3): return 0x10;
                case (4): return 0x08;
                case (5): return 0x04;
                case (6): return 0x02;
                case (7): return 0x01;
                default: return 0x0;
            }
        }

        private byte GetNibleMask()
        {
            if (bitPosition == 0) return 0xF0;
            else return 0xF;
        }

        private byte GetCustomMask(int count)
        {
            switch (count)
            {
                case (2): return (byte)(0xC0 >> bitPosition);
                case (3): return (byte)(0xE0 >> bitPosition);
                case (5): return (byte)(0xF8 >> bitPosition);
                case (6): return (byte)(0xFC >> bitPosition);
                case (7): return (byte)(0xFE >> bitPosition);
                default: throw new ArgumentException();
            }
        }
    }
}

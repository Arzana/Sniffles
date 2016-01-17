using Snifles.Data;
using System;
using System.Net;

namespace Snifles.Application_Layer
{
    public sealed class DnsHeader
    {
        public const int BYTE_COUNT = 12;

        public readonly ushort Indentifier;

        public readonly bool QueryOrResponseFlag;
        public readonly OpCode OperationCode;
        public readonly bool AuthoritativeAnswer;
        public readonly bool Turncation;
        public readonly bool RecursionDesired;

        public readonly bool RecursionAvailable;

        public readonly RCode ResponseCode;

        public readonly ushort QuestionCount;
        public readonly ushort AnswerCount;
        public readonly ushort AuthorityCount;
        public readonly ushort AdditionalCount;

        public readonly byte[] raw;
        public readonly byte[] data;

        public DnsHeader(byte[] byIpData, int start, int bytesReceived)
        {
            NetBinaryReader br = new NetBinaryReader(byIpData, start);

            Indentifier = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            QueryOrResponseFlag = br.ReadBit();
            OperationCode = (OpCode)br.ReadNible();
            AuthoritativeAnswer = br.ReadBit();
            Turncation = br.ReadBit();
            RecursionDesired = br.ReadBit();

            RecursionAvailable = br.ReadBit();
            br.ReadCustomAmount(3);
            ResponseCode = (RCode)br.ReadNible();

            QuestionCount = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            AnswerCount = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            AuthorityCount = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());
            AdditionalCount = (ushort)IPAddress.NetworkToHostOrder(br.ReadInt16());

            raw = new byte[BYTE_COUNT];
            Array.Copy(byIpData, start, raw, 0, raw.Length);

            data = new byte[bytesReceived - start - raw.Length];
            Array.Copy(byIpData, start + raw.Length, data, 0, data.Length);
        }
    }
}

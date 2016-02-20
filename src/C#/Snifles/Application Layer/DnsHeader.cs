using Snifles.Data;
using System;

namespace Snifles.Application_Layer
{
    public sealed class DnsHeader
    {
        public const int OCTET_COUNT = 12;

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

        public readonly byte[] data;

        public DnsHeader(byte[] byIpData, int start, int bytesReceived)
        {
            NetBinaryReader nbr = new NetBinaryReader(byIpData, start);

            Indentifier = nbr.ReadUInt16();

            QueryOrResponseFlag = nbr.ReadBit();
            OperationCode = (OpCode)nbr.ReadNible();
            AuthoritativeAnswer = nbr.ReadBit();
            Turncation = nbr.ReadBit();
            RecursionDesired = nbr.ReadBit();

            RecursionAvailable = nbr.ReadBit();
            nbr.ReadCustomAmount(3);
            ResponseCode = (RCode)nbr.ReadNible();

            QuestionCount = nbr.ReadUInt16();
            AnswerCount = nbr.ReadUInt16();
            AuthorityCount = nbr.ReadUInt16();
            AdditionalCount = nbr.ReadUInt16();

            data = new byte[bytesReceived - start - OCTET_COUNT];
            Array.Copy(byIpData, start + OCTET_COUNT, data, 0, data.Length);
        }
    }
}

using Snifles.Data;
using System.Collections.Generic;
using System.Diagnostics;

namespace Snifles.Application_Layer
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class DnsQuestion
    {
        public int ByteCount { get { return 5 + QueriedDomainName.Length; } }

        public readonly string QueriedDomainName;
        public readonly QType Type;
        public readonly DnsClass Class;

        public DnsQuestion(NetBinaryReader nbr)
        {
            List<string> labels = new List<string>();
            byte nameLength;

            while ((nameLength = nbr.ReadByte()) != 0)
            {
                string label = string.Empty;

                for (int i = 0; i < nameLength; i++)
                {
                    label += (char)nbr.ReadByte();
                }

                labels.Add(label);
            }

            QueriedDomainName = string.Join(".", labels);
            Type = (QType)nbr.ReadUInt16();

            ushort rawClass = nbr.ReadUInt16();
            if (rawClass > 65279) rawClass = 0;
            else if (rawClass > 4 && rawClass < 252) rawClass = 2;
            else if (rawClass > 255 && rawClass < 65280) rawClass = 2;
            Class = (DnsClass)rawClass;
        }

        public override string ToString()
        {
            return $"{Type} To {QueriedDomainName}";
        }
    }
}
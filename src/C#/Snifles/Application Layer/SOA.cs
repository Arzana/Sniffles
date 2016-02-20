using Snifles.Data;

namespace Snifles.Application_Layer
{
    public struct SOA
    {
        public readonly string PrimaryNS;
        public readonly string AdminMB;
        public readonly uint SerialNbr;
        public readonly uint RefreshInterval;
        public readonly uint RetryInterval;
        public readonly uint ExpirationLimit;
        public readonly uint MaxTTL;

        public SOA(NetBinaryReader nbr)
        {
            PrimaryNS = nbr.ReadLblOrPntString();
            AdminMB = nbr.ReadLblOrPntString();
            SerialNbr = nbr.ReadUInt32();
            RefreshInterval = nbr.ReadUInt32();
            RetryInterval = nbr.ReadUInt32();
            ExpirationLimit = nbr.ReadUInt32();
            MaxTTL = nbr.ReadUInt32();
        }
    }
}
namespace Snifles
{
    public abstract class ProtocolHeader
    {
        public ushort Checksum { get; protected set; }
    }
}
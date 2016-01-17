namespace Snifles.Application_Layer
{
    public enum DnsClass : ushort
    {
        Reserved = 0,       // 0, 65280-65535
        Internet = 1,
        Unassigned =  2,    // 2, 5-253, 256-65279
        Chaos = 3,
        Hesiod = 4,
        None = 254,
        Any = 255,
    }
}
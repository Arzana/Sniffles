namespace Snifles.Application_Layer
{
    public enum QType : ushort  // most used, all: http://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml
    {
        Reserved = 0,
        A = 1,
        NS = 2,
        CNAME = 5,
        SOA = 6,
        WKS = 11,
        PTR = 12,
        MX = 15,
        AAAA = 28,
        SRV = 33,
        A6 = 38,
        ANY = 255
    }
}
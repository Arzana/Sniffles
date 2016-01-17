namespace Snifles.Internet_Layer
{
    public enum MessageType : byte
    {
        EchoReply = 0,
        Unassigned = 1,    // 1,2,7
        DestinationUnreachable = 3,
        SourceQuench = 4,
        Redirect = 5,
        AlternateHostAddress = 6,
        Echo = 8,
        RouterAdvertisement = 9,
        RouterSelection = 10,
        TimeExceeded = 11,
        ParameterProblem = 12,
        Timestamp = 13,
        TimestampReply = 14,
        InformationRequest = 15,
        InformationReply = 16,
        AddressMaskRequest = 17,
        AddressMaskReply = 18,
        ReservedForSecurity = 19,
        ReservedForRobustnessExperiment = 20, // 20-29
        Traceroute = 30,
        DatagramConversionError = 31,
        MobileHostRedirect = 32,
        IPv6WhereAreYou = 33,
        IPv6IAmHere = 34,
        MobileRegistrationRequest = 35,
        MobileRegistrationReply = 36,
        DomainNameRequest = 37,
        DomainNameReply = 38,
        SKIP = 39,
        Photuris = 40,
        Reserved = 41 // 41-255
    }
}
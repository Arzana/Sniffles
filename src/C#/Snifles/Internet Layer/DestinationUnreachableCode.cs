namespace Snifles.Internet_Layer
{
    public enum DestinationUnreachableCode : byte
    {
        NetUnreachable,
        HostUnreachable,
        ProtocolUnreachable,
        PortUnreachable,
        FragmentationNeeded,
        SourceRouterFailed,
        DestinationNetworkUnknown,
        DestinationHostUnknown,
        SourceHostIsolated,
        NetworkAdministrativelyProhibited,
        HostAdministrativelyProhibited,
        NetworkUnreachableForServiceType,
        HostUnreachableForServiceType,
        CommunicationAdministrativelyProhibited,
        HostPrecedenceViolation,
        PrecedenceCutoff
    }
}
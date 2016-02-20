namespace Snifles.Application_Layer
{
    public enum RCode : byte
    {
        NoError,
        FormatError,
        ServerFailure,
        NameError,
        NotImplemented,
        Refused,
        YxDomain,
        YxrrSet,
        NxrrSet,
        NotAuthorized,
        NotZone
    }
}
namespace Snifles.Application_Layer
{
    public enum OpCode : byte
    {
        Query,
        IQuery,
        Status,
        Reversed,
        Notify,
        Update
    }
}
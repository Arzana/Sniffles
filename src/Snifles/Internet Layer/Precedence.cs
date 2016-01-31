namespace Snifles.Internet_Layer
{
    public enum Precedence : byte
    {
        Routine,
        Priority,
        Immediate,
        Flash,
        FlashOverride,
        Critical,
        Internetwork,
        NetworkControl
    }
}
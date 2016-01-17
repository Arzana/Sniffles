namespace Snifles.Internet_Layer
{
    public enum Precedence : byte
    {
        NetworkControl = 111,
        InternetwokControl = 110,
        CriticOrEcp = 101,
        FlashOverride = 100,
        Flash = 011,
        Immediate = 010,
        Priority = 001,
        Routine = 0
    }
}
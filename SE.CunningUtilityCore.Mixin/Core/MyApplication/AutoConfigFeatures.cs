using System;

namespace IngameScript
{
    [Flags]
    public enum AutoConfigFeatures
    {
        None = 0,
        Scheduler = 1 << 1,
        Commands = 1 << 3,
        IGC = 1 << 4,
        All = Scheduler | Commands | IGC
    }
}
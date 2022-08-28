using System;

namespace Battle.data.enums.bomb
{
    [Flags]
    public enum BombFlag
    {
        Start = 1,
        Stop = 2,
        Defuse = 4,
        Unk1 = 8
    }
}
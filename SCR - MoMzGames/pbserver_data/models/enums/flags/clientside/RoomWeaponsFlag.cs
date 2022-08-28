using System;

namespace Core.models.enums.flags
{
    [Flags]
    public enum RoomWeaponsFlag
    {
        Mode_Barefist = 128,
        Mode_870MCS = 64,
        Mode_SSG69 = 32,
        RPG7 = 16,
        Primary = 8,
        Secondary = 4,
        Melee = 2,
        Grenade = 1
    }
}
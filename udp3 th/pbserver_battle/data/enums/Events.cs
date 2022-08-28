using System;

namespace Battle.data.enums
{
    [Flags]
    public enum Events : uint
    {
        ActionState = 1,
        Animation = 2,
        PosRotation = 4,
        OnLoadObject = 8,
        Unk1 = 0x10,
        RadioChat = 0x20,
        WeaponSync = 0x40,
        WeaponRecoil = 0x80,
        LifeSync = 0x100,
        Suicide = 0x200,
        Mission = 0x400,
        TakeWeapon = 0x800,
        DropWeapon = 0x1000,
        FireSync = 0x2000,
        AIDamage = 0x4000,
        NormalDamage = 0x8000,
        BoomDamage = 0x10000,
        Unk3 = 0x20000,
        Death = 0x40000,
        SufferingDamage = 0x80000,
        PassPortal = 0x100000,
    }
}
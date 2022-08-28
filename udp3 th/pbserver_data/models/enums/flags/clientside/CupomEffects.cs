using System;

namespace Core.models.enums.flags
{
    [Flags]
    public enum CupomEffects
    {
        Colete90 = 1,
        Ketupat = 2,
        Colete20 = 4,
        HollowPointPlus = 8,
        Colete10 = 0x10,
        HP5 = 0x20,
        HollowPointF = 0x40,
        ExtraGrenade = 0x80,
        C4SpeedKit = 0x100,
        HollowPoint = 0x200,
        IronBullet = 0x400,
        Colete5 = 0x800,
        Invulnerability = 0x1000,
        HP10 = 0x2000,
        FastReload = 0x4000,
        FastChange = 0x8000,
        FlashProtect = 0x10000,
        TakeDrop = 0x20000,
        Ammo40 = 0x40000,
        Respawn30 = 0x80000,
        Respawn50 = 0x100000,
        Respawn100 = 0x200000,
        Ammo10 = 0x400000,
        ExtraThrowGrenade = 0x800000,
    }
}
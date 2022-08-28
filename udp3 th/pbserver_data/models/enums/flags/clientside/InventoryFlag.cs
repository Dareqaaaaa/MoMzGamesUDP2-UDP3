using System;

namespace Core.models.enums.flags
{
    [Flags]
    public enum InventoryFlag
    {
        Character = 1,
        Weapon = 2,
        Item = 4
    }
}
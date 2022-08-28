using System;

namespace Core.models.enums.flags
{
    [Flags]
    public enum RoomStageFlag
    {
        LockRoom = 0x80,
        Private = 0x04,
        Random = 0x02
    }
}
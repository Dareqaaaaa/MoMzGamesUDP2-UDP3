using System;

namespace Core.models.enums.flags
{
    [Flags]
    public enum DeadEnum
    {
        isAlive = 1,
        isDead = 2,
        useChat = 4
    }
}
/*
 * Arquivo: ResultIcon.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 28/03/2017
 * Sintam inveja, não nos atinge
 */

using System;

namespace Core.models.enums.flags
{
    [Flags]
    public enum ResultIcon
    {
        None = 0,
        Pc = 1,
        PcPlus = 2,
        Item = 4,
        Event = 8
    }
}
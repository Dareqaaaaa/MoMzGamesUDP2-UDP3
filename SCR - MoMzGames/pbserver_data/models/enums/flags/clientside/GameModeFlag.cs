/*
 * Arquivo: GameModeFlag.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 09/08/2017
 * Sintam inveja, não nos atinge
 */

using System;

namespace Core.models.enums.flags
{
    [Flags]
    public enum GameModeFlag
    {
        TDM_Challenge = 1,
        SniperMode = 4, //Sniper Mode/Tutorial
        TDM_Supression = 8, //Supressão
        HeadHunter = 32,
        ShotgunMode = 0x80,
        CrossCounter = 0x100, //Knuckle mode
        Tutorial = 0x200,
        Zombie = 0x400,
        DURO_DE_MATAR = 0x800,
        Chaos = 0x1000
    }
}
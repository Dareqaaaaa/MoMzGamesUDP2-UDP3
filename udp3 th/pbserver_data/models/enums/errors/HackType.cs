namespace Core.models.enums.errors
{
    public enum HackType
    {
        Default = 0,
        AmmoCount = 1, //(410.0) GrenadeCount/RpgCount
        Unk3 = 16, //(1014.0)
        Unk2 = 18, //(1016.0)
        Unk = 19, //(1017.0) (Valor > 2.0)
        Stage = 21, //(1019.0) Se atirar em mais de 1 pessoa, e não for Shotgun
        UnkSpeed = 23, //(1021.0) - PosRotation (Valor > 1.0) Velocidade??
        Unk4 = 24, //(1022.0)
        UnkInvalidUsageEffect = 26, //(1024.0) - Uso de item inválido (Deathtype!=10) Provável que se usar cura sem o kit médico
        UnkDistance = 28, //(1026.0) - Atirar (Valor > 40.0) Distância???
        Unk5 = 32, //(1030.0)
        JumpCheck = 34, //(1032.0)
    }
}
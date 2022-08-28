/*
 * Arquivo: ActionModel.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 04/07/2017
 * Sinta inveja, não nos atinge
 */

using Battle.data.enums;

namespace Battle.data.models
{
    public class ActionModel
    {
        public ushort _slot, _lengthData;
        public Events _flags;
        public P2P_SUB_HEAD _type;
        public byte[] _data;
    }
}
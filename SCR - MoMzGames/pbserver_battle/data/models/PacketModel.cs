/*
 * Arquivo: PacketModel.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 08/01/2017
 * Sinta inveja, não nos atinge
 */

using System;
using System.Collections.Generic;

namespace Battle.data.models
{
    public class PacketModel
    {
        public int _opcode, _slot, _round, _length, _accountId, _unkInfo2, _respawnNumber, _roundNumber;
        public float _time;
        public byte[] _data, _withEndData, _noEndData;
        public DateTime _receiveDate;
    }
}
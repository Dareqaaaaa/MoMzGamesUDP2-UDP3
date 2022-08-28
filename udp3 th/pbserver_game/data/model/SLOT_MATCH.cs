/*
 * Arquivo: SLOT_MATCH.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 28/04/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.enums.match;

namespace Game.data.model
{
    public class SLOT_MATCH
    {
        public SlotMatchState state;
        public long _playerId, _id;
        public SLOT_MATCH(int slot)
        {
            _id = slot;
        }
    }
}
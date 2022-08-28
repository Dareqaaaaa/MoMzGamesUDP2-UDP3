/*
 * Arquivo: PLAYER_CONFIG_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 11/01/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.players;
using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_USER_EFFECTS_PAK : SendPacket
    {
        private int _type;
        private PlayerBonus _bonus;
        public BASE_USER_EFFECTS_PAK(int type, PlayerBonus bonus)
        {
            _type = type;
            _bonus = bonus;
        }

        public override void write()
        {
            writeH(2638);
            writeH((ushort)_type);
            writeD(_bonus.fakeRank);
            writeD(_bonus.fakeRank);
            writeS(_bonus.fakeNick, 33);
            writeH((short)_bonus.sightColor);
        }
    }
}
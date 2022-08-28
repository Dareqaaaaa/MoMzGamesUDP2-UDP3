using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_PLAYER_CLEAN_INVITES_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_PLAYER_CLEAN_INVITES_PAK(uint error)
        {
            _erro = error;
        }

        public override void write()
        {
            writeH(1319);
            writeD(_erro);
        }
    }
}
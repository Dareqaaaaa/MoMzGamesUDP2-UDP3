using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_ACCEPTED_BATTLE_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_WAR_ACCEPTED_BATTLE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1559);
            writeD(_erro);
        }
    }
}
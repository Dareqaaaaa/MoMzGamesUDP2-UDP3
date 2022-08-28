using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_RECUSED_BATTLE_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_WAR_RECUSED_BATTLE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1560);
            writeD(_erro);
        }
    }
}
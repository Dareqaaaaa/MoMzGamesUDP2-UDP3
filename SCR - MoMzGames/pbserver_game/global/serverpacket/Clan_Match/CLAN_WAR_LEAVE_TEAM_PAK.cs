using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_LEAVE_TEAM_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_WAR_LEAVE_TEAM_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1551);
            writeD(_erro);
        }
    }
}
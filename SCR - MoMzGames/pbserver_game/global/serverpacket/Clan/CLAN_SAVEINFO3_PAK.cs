using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_SAVEINFO3_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_SAVEINFO3_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1373);
            writeD(_erro);
        }
    }
}
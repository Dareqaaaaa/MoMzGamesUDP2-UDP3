using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CREATE_INVITE_PAK : SendPacket
    {
        private int _clanId;
        private uint _erro;
        public CLAN_CREATE_INVITE_PAK(uint erro, int clanId)
        {
            _erro = erro;
            _clanId = clanId;
        }

        public override void write()
        {
            writeH(1317);
            writeD(_erro);
            if (_erro == 0)
                writeD(_clanId);
        }
    }
}
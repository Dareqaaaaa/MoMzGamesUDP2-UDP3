using Core.server;

namespace Game.global.serverpacket
{
    public class ROOM_INVITE_RETURN_PAK : SendPacket
    {
        private uint _erro;
        public ROOM_INVITE_RETURN_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(3885);
            writeD(_erro);
        }
    }
}
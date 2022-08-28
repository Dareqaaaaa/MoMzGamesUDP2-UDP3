using Core.server;

namespace Game.global.serverpacket
{
    public class ROOM_CLOSE_SLOT_PAK : SendPacket
    {
        private uint _erro;
        public ROOM_CLOSE_SLOT_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(3850);
            writeD(_erro);
        }
    }
}
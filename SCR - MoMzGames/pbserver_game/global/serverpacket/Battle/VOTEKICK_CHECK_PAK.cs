using Core;
using Core.server;

namespace Game.global.serverpacket
{
    public class VOTEKICK_CHECK_PAK : SendPacket
    {
        private uint _erro;
        public VOTEKICK_CHECK_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(3397);
            writeD(_erro);
        }
    }
}
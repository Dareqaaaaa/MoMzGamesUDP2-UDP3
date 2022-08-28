using Core.server;

namespace Game.global.serverpacket
{
    public class HELPER_PAK : SendPacket
    {
        private ushort _packet;
        public HELPER_PAK(ushort packet)
        {
            _packet = packet;
        }

        public override void write()
        {
            writeH(_packet);
            writeD(0);
            writeC(1);
        }
    }
} //3879 - 4v4error?
  //553 = RECEBER PRESENTE
  //2630 - zera minhas info e pede 2561
  //2685 - log do usuário é salvo
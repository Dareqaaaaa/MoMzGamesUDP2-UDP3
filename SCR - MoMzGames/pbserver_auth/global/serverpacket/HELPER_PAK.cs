using Core.server;

namespace Auth.global.serverpacket
{
    public class HELPER_PAK : SendPacket
    {
        private short _packet;
        public HELPER_PAK(short packet)
        {
            _packet = packet;
        }

        public override void write()
        {
            writeH(_packet);
            writeD(0); //id do cupom ativo
        }
    }
} //3879 - 4v4error?
  //553 = RECEBER PRESENTE
  //2630 - zera minhas info e pede 2561
  //2685 - log do usuário é salvo
using Core.server;

namespace Game.global.serverpacket
{
    public class SHOP_TEST2_PAK : SendPacket
    {
        public SHOP_TEST2_PAK()
        {
        }
        public override void write()
        {
            writeH(567); //Não existe
            writeD(0);
            writeD(0);
            writeD(0);
            writeD(44); //356
        }
    }
}
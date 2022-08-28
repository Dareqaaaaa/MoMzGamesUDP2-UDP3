using Core.managers;
using Core.server;

namespace Game.global.serverpacket
{
    public class SHOP_GET_ITEMS_PAK : SendPacket
    {
        private int _tudo;
        private ShopData data;
        public SHOP_GET_ITEMS_PAK(ShopData data, int tudo)
        {
            this.data = data;
            _tudo = tudo;
        }
        public override void write()
        {
            writeH(525); //1111 itens por página
            writeD(_tudo);
            writeD(data.ItemsCount);
            writeD(data.Offset);
            writeB(data.Buffer);
            writeD(44);
        }
    }
}
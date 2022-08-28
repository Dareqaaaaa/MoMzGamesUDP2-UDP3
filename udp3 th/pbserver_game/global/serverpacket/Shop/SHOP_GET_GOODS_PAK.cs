using Core.managers;
using Core.server;

namespace Game.global.serverpacket
{
    public class SHOP_GET_GOODS_PAK : SendPacket
    {
        private int _tudo;
        private ShopData data;
        public SHOP_GET_GOODS_PAK(ShopData data, int tudo)
        {
            this.data = data; 
            _tudo = tudo;
        }
        public override void write()
        {
            writeH(523); //592 itens por página
            writeD(_tudo);
            writeD(data.ItemsCount);
            writeD(data.Offset);
            writeB(data.Buffer);
            writeD(44); //356
        }
    }
}
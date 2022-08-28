using Core.server;

namespace Game.global.serverpacket
{
    public class SHOP_GET_REPAIR_PAK : SendPacket
    {
        public SHOP_GET_REPAIR_PAK()
        {
        }
        public override void write()
        {
            writeH(559);
            writeD(0); //COUNT
            writeD(0); //COUNT
            writeD(0); //Offset

            //writeD(100003188); //ItemId?
            //writeD(59); //GoldRepairPrice?
            //writeD(0); //CashRepairPrice?
            //writeD(100); //Max durability?
            /*
             * ID DO ITEM - WRITED
             * UNK?? - WRITED (Geralmente varia) Ou é esse que tem valor
             * UNK?? - WRITED (Geralmente varia) Repair Type (0 = ? | 1= GOLD | 2=CASH)
             * COUNT - WRITED (100)
             * 
             * 
             */

            writeD(44); //356
        }
    }
}
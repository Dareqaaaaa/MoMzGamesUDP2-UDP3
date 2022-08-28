using Core.models.account;
using Core.server;

namespace Game.global.serverpacket
{
    public class BOX_MESSAGE_GIFT_RECEIVE_PAK : SendPacket
    {
        private Message gift;
        public BOX_MESSAGE_GIFT_RECEIVE_PAK(Message gift)
        {
            this.gift = gift;
        }

        public override void write()
        {
            writeH(553);
            writeD(gift.object_id); //MsgId?
            writeD((uint)gift.sender_id); //Good
            writeD(gift.state); //?
            writeD((uint)gift.expireDate); //Data do término
            writeC((byte)(gift.sender_name.Length + 1));
            writeS(gift.sender_name, gift.sender_name.Length + 1);
            writeC(6);
            writeS("EVENT", 6); //Mensagem
            /*
             * 29 02 EF 11   ·|×···u····)·ï·
00000090   21 00 A9 30 9A 00 00 00  00 00 BF 6D B2 65 03 47   !·©0·····¿m²e·G
000000A0   4D 00 06 45 56 45 4E 54  00 
             */
        }
    }
}
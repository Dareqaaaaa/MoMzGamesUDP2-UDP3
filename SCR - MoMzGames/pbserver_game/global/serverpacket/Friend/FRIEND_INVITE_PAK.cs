using Core.server;

namespace Game.global.serverpacket
{
    public class FRIEND_INVITE_PAK : SendPacket
    {
        private uint _erro;
        public FRIEND_INVITE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(283);
            writeD(_erro);
            /*
             * 0x80001038 STR_TBL_GUI_BASE_FAIL_REQUEST_FRIEND_BY_LIMIT
             * 0x80001038 STR_TBL_GUI_BASE_NO_MORE_GET_FRIEND_STATE
             * 0x80001041 STR_TBL_GUI_BASE_ALREADY_REGIST_FRIEND_LIST
             * 
             */
        }
    }
}
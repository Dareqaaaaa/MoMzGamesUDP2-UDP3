using Core.server;

namespace Game.global.serverpacket
{
    public class FRIEND_INVITE_FOR_ROOM_PAK : SendPacket
    {
        private uint _erro;
        public FRIEND_INVITE_FOR_ROOM_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(276);
            writeD(_erro);
            /*
             * 2147495938 STR_TBL_NETWORK_FAIL_INVITED_USER
             * 2147495939 STR_TBL_NETWORK_FAIL_INVITED_USER_IN_CLAN_MATCH
             */
        }
    }
}
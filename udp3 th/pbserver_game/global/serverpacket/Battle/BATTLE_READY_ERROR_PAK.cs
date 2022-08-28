using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_READY_ERROR_PAK : SendPacket
    {
        private uint _erro;
        public BATTLE_READY_ERROR_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(3332);
            //Se for tutorial, não lê o resto.
            writeD(_erro); //2147487858 - não pode iniciar sem ser 4x4 - CLÃ
            /*
             * 0x80001008 STBL_IDX_EP_ROOM_NO_REAL_IP_S
             * 0x80001009 STBL_IDX_EP_ROOM_NO_READY_TEAM_S
             * 0x80001012 STBL_IDX_EP_ROOM_NO_START_FOR_UNDER_NAT
             * 0x80001071 STBL_IDX_EP_ROOM_NO_START_FOR_NO_CLAN_TEAM
             * 0x80001072 STBL_IDX_EP_ROOM_NO_START_FOR_TEAM_NOTFULL
             * 0x80001098 STBL_IDX_EP_ROOM_NO_START_FOR_NOT_ALL_READY
             * 0x800010AB STBL_IDX_EP_ROOM_ERROR_READY_WEAPON_EQUIP
             */
        }
    }
}
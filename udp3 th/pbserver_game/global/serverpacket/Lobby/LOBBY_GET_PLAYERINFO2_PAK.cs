using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class LOBBY_GET_PLAYERINFO2_PAK : SendPacket
    {
        private Account ac;
        public LOBBY_GET_PLAYERINFO2_PAK(long player)
        {
            ac = AccountManager.getAccount(player, true);
        }

        public override void write()
        {
            writeH(3100);
            if (ac != null && ac._equip != null)
            {
                writeD(ac._equip._primary);
                writeD(ac._equip._secondary);
                writeD(ac._equip._melee);
                writeD(ac._equip._grenade);
                writeD(ac._equip._special);
                writeD(ac._equip._red);
                writeD(ac._equip._blue);
                writeD(ac._equip._helmet);
                writeD(ac._equip._beret);
                writeD(ac._equip._dino);
            }
            else
            {
                writeD(0);
                writeD(601002003);
                writeD(702001001);
                writeD(803007001);
                writeD(904007002);
                writeD(1001001005);
                writeD(1001002006);
                writeD(1102003001);
                writeD(0);
                writeD(1006003041);
            }
            writeD(0); //Count de writeD
        }
    }
}
using Core.models.account.title;
using Core.models.room;
using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_READYBATTLE2_PAK : SendPacket
    {
        private SLOT slot;
        private PlayerTitles title;
        public BATTLE_READYBATTLE2_PAK(SLOT slot, PlayerTitles title)
        {
            this.slot = slot;
            this.title = title;
        }

        public override void write()
        {
            if (slot._equip == null)
                return;
            writeH(3427);
            writeC((byte)slot._id);
            writeD(slot._equip._red);
            writeD(slot._equip._blue);
            writeD(slot._equip._helmet);
            writeD(slot._equip._beret);
            writeD(slot._equip._dino);
            writeD(slot._equip._primary);
            writeD(slot._equip._secondary);
            writeD(slot._equip._melee);
            writeD(slot._equip._grenade);
            writeD(slot._equip._special);
            writeD(0);
            writeC((byte)title.Equiped1);
            writeC((byte)title.Equiped2);
            writeC((byte)title.Equiped3);
            if (GameManager.Config.ClientVersion == "1.15.42")
                writeD(0); //Somente 1.15.42
        }
    }
}
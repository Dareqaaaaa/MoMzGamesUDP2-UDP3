using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_DEATH_PAK : SendPacket
    {
        private Room room;
        private FragInfos kills;
        private SLOT killer;
        private bool isBotMode;
        public BATTLE_DEATH_PAK(Room r, FragInfos kills, SLOT killer, bool isBotMode)
        {
            room = r;
            this.kills = kills;
            this.killer = killer;
            this.isBotMode = isBotMode;
        }

        public override void write()
        {
            writeH(3355);
            writeC((byte)kills.killingType);
            writeC(kills.killsCount);
            writeC(kills.killerIdx);
            writeD(kills.weapon);
            writeT(kills.x);
            writeT(kills.y);
            writeT(kills.z);
            writeC(kills.flag);
            for (int i = 0; i < kills.frags.Count; i++)
            {
                Frag frag = kills.frags[i];
                writeC(frag.victimWeaponClass);
                writeC(frag.hitspotInfo);
                writeH((short)frag.killFlag);
                writeC(frag.flag);
                writeT(frag.x);
                writeT(frag.y);
                writeT(frag.z);
            }
            writeH((short)room._redKills);
            writeH((short)room._redDeaths);
            writeH((short)room._blueKills);
            writeH((short)room._blueDeaths);
            for (int i = 0; i < 16; i++)
            {
                SLOT slot = room._slots[i];
                writeH((short)slot.allKills);
                writeH((short)slot.allDeaths);
            }
            if (killer.killsOnLife == 2) writeC(1);
            else if (killer.killsOnLife == 3) writeC(2);
            else if (killer.killsOnLife > 3) writeC(3);
            else writeC(0);
            writeH((ushort)kills.Score);
            if ((RoomType)room.room_type == RoomType.Boss)
            {
                writeH((ushort)room.red_dino);
                writeH((ushort)room.blue_dino);
            }
        }
    }
}
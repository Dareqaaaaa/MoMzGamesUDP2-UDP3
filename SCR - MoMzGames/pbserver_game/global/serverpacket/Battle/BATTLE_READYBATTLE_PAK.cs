using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_READYBATTLE_PAK : SendPacket
    {
        private int PlayersCount;
        private Room room;
        private byte[] Data;
        public BATTLE_READYBATTLE_PAK(Room r)
        {
            room = r;
            if (ConfigGS.isTestMode && ConfigGS.udpType == SERVER_UDP_STATE.RELAY)
            {
                room._slots[LoggerGS.TestSlot]._playerId = 0;
                room._slots[LoggerGS.TestSlot].state = SLOT_STATE.EMPTY;
            }
            string client = GameManager.Config.ClientVersion;
            using (SendGPacket pk = new SendGPacket())
            {
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = room._slots[i];
                    if ((int)slot.state >= 8 && slot._equip != null)
                    {
                        Account player = room.getPlayerBySlot(slot);
                        if (player != null && player._slotId == i)
                        {
                            WriteSlotInfo(slot, player, client, pk);
                            PlayersCount++;
                        }
                    }
                }
                Data = pk.mstream.ToArray();
            }
        }
        private void WriteSlotInfo(SLOT s, Account p, string client, SendGPacket pk)
        {
            pk.writeC((byte)s._id);
            pk.writeD(s._equip._red);
            pk.writeD(s._equip._blue);
            pk.writeD(s._equip._helmet);
            pk.writeD(s._equip._beret);
            pk.writeD(s._equip._dino);
            pk.writeD(s._equip._primary);
            pk.writeD(s._equip._secondary);
            pk.writeD(s._equip._melee);
            pk.writeD(s._equip._grenade);
            pk.writeD(s._equip._special);
            pk.writeD(0); //??9
            if (p != null)
            {
                pk.writeC((byte)p._titles.Equiped1);
                pk.writeC((byte)p._titles.Equiped2);
                pk.writeC((byte)p._titles.Equiped3);
            }
            else
                pk.writeB(new byte[3]);
         if (client == "1.15.42")
                pk.writeD(0); //Somente 1.15.42
        }
        public override void write()
        {
            writeH(3426);
            writeH((short)room.mapId);
            writeC(room.stage4v4);
            writeC(room.room_type);
            writeC((byte)PlayersCount);
            writeB(Data);
        }
    }
}
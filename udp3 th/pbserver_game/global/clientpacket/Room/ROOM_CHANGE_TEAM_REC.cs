using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.global.clientpacket
{
    public class ROOM_CHANGE_TEAM_REC : ReceiveGamePacket
    {
        private List<SLOT_CHANGE> changeList = new List<SLOT_CHANGE>();
        public ROOM_CHANGE_TEAM_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                Room r = p == null ? null : p._room;
                if (r != null && r._leader == p._slotId && r._state == RoomState.Ready && !r.changingSlots)
                {
                    Monitor.Enter(r._slots);
                    r.changingSlots = true;
                    foreach (int slotIdx in r.RED_TEAM)
                    {
                        int NewId = (slotIdx + 1);
                        if (slotIdx == r._leader)
                            r._leader = NewId;
                        else if (NewId == r._leader)
                            r._leader = slotIdx;
                        r.SwitchSlots(changeList, NewId, slotIdx, true);
                    }
                    if (changeList.Count > 0)
                    {
                        using (ROOM_CHANGE_SLOTS_PAK packet = new ROOM_CHANGE_SLOTS_PAK(changeList, r._leader, 2))
                        {
                            byte[] data = packet.GetCompleteBytes("ROOM_CHANGE_TEAM_REC");
                            foreach (Account ac in r.getAllPlayers())
                            {
                                ac._slotId = AllUtils.getNewSlotId(ac._slotId);
                                Logger.LogProblems("[ROOM_CHANGE_TEAM_REC] Jogador '" + ac.player_id + "' '" + ac.player_name + "'; NewSlot: " + ac._slotId, "errorC");
                                ac.SendCompletePacket(data);
                            }
                        }
                    }
                    r.changingSlots = false;
                    Monitor.Exit(r._slots);
                }
            }
            catch (Exception ex)
            {
                Logger.info("ROOM_CHANGE_TEAM_REC: " + ex.ToString());
            }
        }
    }
}
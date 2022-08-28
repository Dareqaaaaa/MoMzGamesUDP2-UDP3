using Core;
using Core.models.enums;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class BATTLE_SENDPING_REC : ReceiveGamePacket
    {
        private byte[] slots;
        private int ReadyPlayersCount;
        public BATTLE_SENDPING_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slots = readB(16);
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                    return;
                Room room = player._room;
                if (room != null && room._slots[player._slotId].state >= SLOT_STATE.BATTLE_READY)
                {
                    if ((int)room._state == 5)
                        room._ping = slots[room._leader];
                    using (BATTLE_SENDPING_PAK packet = new BATTLE_SENDPING_PAK(slots))
                    {
                        List<Account> players = room.getAllPlayers(SLOT_STATE.READY, 1);
                        if (players.Count == 0)
                            return;

                        byte[] data = packet.GetCompleteBytes("BATTLE_SENDPING_REC");
                        foreach (Account pR in players)
                        {
                            if ((int)room._slots[pR._slotId].state >= 12)
                                pR.SendCompletePacket(data);
                            else
                                ++ReadyPlayersCount;
                        }
                    }
                    if (ReadyPlayersCount == 0)
                        room.SpawnReadyPlayers();
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[BATTLE_SENDPING_REC] " + ex.ToString());
            }
        }
    }
}
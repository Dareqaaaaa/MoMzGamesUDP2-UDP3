using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class ROOM_REQUEST_HOST_REC : ReceiveGamePacket
    {
        public ROOM_REQUEST_HOST_REC(GameClient client, byte[] data)
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
                if (_client == null)
                    return;
                Account p = _client._player;
                Room r = p == null ? null : p._room;
                if (r != null)
                {
                    if (r._state != RoomState.Ready || r._leader == p._slotId)
                        return;
                    List<Account> players = r.getAllPlayers();
                    if (players.Count == 0)
                        return;

                    if ((int)p.access >= 4)
                        ChangeLeader(r, players, p._slotId);
                    else
                    {
                        if (!r.requestHost.Contains(p.player_id))
                        {
                            r.requestHost.Add(p.player_id);
                            if (r.requestHost.Count() < (players.Count / 2) + 1)
                            {
                                using (ROOM_GET_HOST_PAK packet = new ROOM_GET_HOST_PAK(p._slotId))
                                    SendPacketToRoom(packet, players);
                            }
                        }
                        if (r.requestHost.Count() >= (players.Count / 2) + 1)
                            ChangeLeader(r, players, p._slotId);
                    }
                }
                else _client.SendPacket(new ROOM_GET_HOST_PAK(0x80000000));
            }
            catch (Exception ex)
            {
                Logger.info("ROOM_REQUEST_HOST_REC: " + ex.ToString());
            }
        }
        private void ChangeLeader(Room r, List<Account> players, int slotId)
        {
            r.setNewLeader(slotId, 0, -1, false);
            using (ROOM_CHANGE_HOST_PAK packet = new ROOM_CHANGE_HOST_PAK(slotId))
                SendPacketToRoom(packet, players);
            r.updateSlotsInfo();
            r.requestHost.Clear();
        }
        private void SendPacketToRoom(SendPacket packet, List<Account> players)
        {
            byte[] data = packet.GetCompleteBytes("ROOM_REQUEST_HOST_REC");
            foreach (Account pR in players)
                pR.SendCompletePacket(data);
        }
    }
}
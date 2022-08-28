using Core;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class LOBBY_ENTER_REC : ReceiveGamePacket
    {
        public LOBBY_ENTER_REC(GameClient client, byte[] data)
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
                Account player = _client._player;
                if (player == null)
                    return;
                player.LastLobbyEnter = DateTime.Now;
                if (player.channelId >= 0)
                {
                    Channel ch = player.getChannel();
                    if (ch != null)
                        ch.AddPlayer(player.Session);
                }
                Room room = player._room;
                if (room != null)
                {
                    if (player._slotId >= 0 && (int)room._state >= 2 && (int)room._slots[player._slotId].state >= 9)
                        goto JumpToPacket;
                    else
                        room.RemovePlayer(player, false);
                }
                AllUtils.syncPlayerToFriends(player, false);
                AllUtils.syncPlayerToClanMembers(player);
                AllUtils.GetXmasReward(player);
            JumpToPacket:
                _client.SendPacket(new LOBBY_ENTER_PAK());
            }
            catch (Exception ex)
            {
                Logger.warning("[LOBBY_ENTER_REC] " + ex.ToString());
            }
        }
    }
}
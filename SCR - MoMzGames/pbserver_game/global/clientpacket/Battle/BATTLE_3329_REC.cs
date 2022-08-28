using Core;
using Core.models.room;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_3329_REC : ReceiveGamePacket
    {
        public BATTLE_3329_REC(GameClient client, byte[] data)
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
                if (p == null)
                    return;
                Room room = p._room;
                Logger.warning("BATTLE_3329_REC. (PlayerID: " + p.player_id + "; Name: " + p.player_name + "; Room: " + (p._room != null ? p._room._roomId : -1) + "; Channel: " + p.channelId + ")");
                if (room != null)
                {
                    Logger.warning("Room3329; BOT: " + room.isBotMode());
                    SLOT slot = room.getSlot(p._slotId);
                    if (slot != null)
                    {
                        Logger.warning("SLOT Id: " + slot._id + "; State: " + slot.state);
                    }
                }
                //p.SendPacket(new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("UnkFunction1")));
                _client.SendPacket(new A_3329_PAK());
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class LOBBY_GET_ROOMINFO_REC : ReceiveGamePacket
    {
        private int roomId;
        public LOBBY_GET_ROOMINFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            roomId = readD();
        }

        public override void run()
        {
            if (_client == null)
                return;
            try
            {
                Account p = _client._player, leader;
                if (p == null)
                    return;
                Channel ch = p.getChannel();
                if (ch != null)
                {
                    Room room = ch.getRoom(roomId);
                    if (room != null && room.getLeader(out leader))
                        _client.SendPacket(new LOBBY_GET_ROOMINFO_PAK(room, leader));
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[LOBBY_GET_ROOMINFO_REC] " + ex.ToString());
            }
        }
    }
}
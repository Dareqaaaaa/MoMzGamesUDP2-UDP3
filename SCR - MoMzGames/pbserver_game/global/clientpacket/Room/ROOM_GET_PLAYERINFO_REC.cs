using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class ROOM_GET_PLAYERINFO_REC : ReceiveGamePacket
    {
        private int slotId;
        public ROOM_GET_PLAYERINFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slotId = readD();
        }

        public override void run()
        {
            Account p = _client._player;
            if (p == null)
                return;
            Room room = p._room;
            try
            {
                _client.SendPacket(new ROOM_GET_PLAYERINFO_PAK(room != null ? room.getPlayerBySlot(slotId) : null));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
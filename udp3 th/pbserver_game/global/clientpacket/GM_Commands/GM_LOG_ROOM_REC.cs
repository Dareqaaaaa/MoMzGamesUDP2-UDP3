using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class GM_LOG_ROOM_REC : ReceiveGamePacket
    {
        private int slot;
        public GM_LOG_ROOM_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slot = readC();
        }

        public override void run()
        {
            Account p = _client._player, pR;
            if (p == null || !p.IsGM())
                return;
            Room room = p._room;
            if (room != null && room.getPlayerBySlot(slot, out pR))
                _client.SendPacket(new GM_LOG_ROOM_PAK(pR));
        }
    }
}
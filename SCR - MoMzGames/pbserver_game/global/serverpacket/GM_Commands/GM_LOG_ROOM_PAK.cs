using Game.data.model;
using Core.server;

namespace Game.global.serverpacket
{
    public class GM_LOG_ROOM_PAK : SendPacket
    {
        private Account player;
        public GM_LOG_ROOM_PAK(Account p)
        {
            player = p;
        }

        public override void write()
        {
            writeH(2687);
            writeD(0);
            writeQ(player.player_id);
        }
    }
}
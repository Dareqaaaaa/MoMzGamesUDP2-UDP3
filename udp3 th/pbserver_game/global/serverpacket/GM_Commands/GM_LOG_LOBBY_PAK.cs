using Game.data.model;
using Core.server;

namespace Game.global.serverpacket
{
    public class GM_LOG_LOBBY_PAK : SendPacket
    {
        private Account player;
        public GM_LOG_LOBBY_PAK(Account p)
        {
            player = p;
        }

        public override void write()
        {
            writeH(2685);
            writeD(0);
            writeQ(player.player_id);
        }
    }
}
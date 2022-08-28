using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class LOBBY_GET_PLAYERINFO2_REC : ReceiveGamePacket
    {
        private uint sessionId;
        public LOBBY_GET_PLAYERINFO2_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            sessionId = readUD();
        }

        public override void run()
        {
            Account player = _client._player;
            if (player == null)
                return;
            long playerId = 0;
            try
            {
                playerId = player.getChannel().getPlayer(sessionId)._playerId;
            }
            catch { }
            _client.SendPacket(new LOBBY_GET_PLAYERINFO2_PAK(playerId));
        }
    }
}
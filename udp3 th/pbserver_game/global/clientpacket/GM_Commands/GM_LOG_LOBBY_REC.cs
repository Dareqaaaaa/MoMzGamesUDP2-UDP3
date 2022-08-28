using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class GM_LOG_LOBBY_REC : ReceiveGamePacket
    {
        private uint sessionId;
        public GM_LOG_LOBBY_REC(GameClient client, byte[] data)
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
            if (player == null || !player.IsGM())
                return;
            Account p = null;
            try
            {
                p = AccountManager.getAccount(player.getChannel().getPlayer(sessionId)._playerId, true);
            }
            catch { }
            if (p != null)
                _client.SendPacket(new GM_LOG_LOBBY_PAK(p));
        }
    }
}
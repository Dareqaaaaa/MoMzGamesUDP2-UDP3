using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class LOBBY_GET_PLAYERINFO_REC : ReceiveGamePacket
    {
        private uint sessionId;
        public LOBBY_GET_PLAYERINFO_REC(GameClient client, byte[] data)
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
            Account p = null;
            try
            {
                p = AccountManager.getAccount(player.getChannel().getPlayer(sessionId)._playerId, true);
            }
            catch { }
            _client.SendPacket(new LOBBY_GET_PLAYERINFO_PAK(p != null ? p._statistic : null));
        }
    }
}
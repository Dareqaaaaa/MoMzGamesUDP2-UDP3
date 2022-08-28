using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class ROOM_INVITE_PLAYERS_REC : ReceiveGamePacket
    {
        private int count;
        private uint erro;
        public ROOM_INVITE_PLAYERS_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            count = readD();
        }

        public override void run()
        {
            Account p = _client._player;
            if (p != null && p._room != null)
            {
                Channel ch = p.getChannel();
                if (ch != null)
                {
                    using (ROOM_INVITE_SHOW_PAK packet = new ROOM_INVITE_SHOW_PAK(p, p._room))
                    {
                        byte[] data = packet.GetCompleteBytes("ROOM_INVITE_PLAYERS_REC");
                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                Account ps = AccountManager.getAccount(ch.getPlayer(readUD())._playerId, true);
                                if (ps != null)
                                    ps.SendCompletePacket(data);
                            }
                            catch { }
                        }
                    }
                }
            }
            else erro = 0x80000000;
            _client.SendPacket(new ROOM_INVITE_RETURN_PAK(erro));
        }
    }
}
using Core.server;
using Core.models.account.clan;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class CLAN_REPLACE_NOTICE_REC : ReceiveGamePacket
    {
        private string clan_news;
        private uint erro;
        public CLAN_REPLACE_NOTICE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            clan_news = readS(readC());
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p != null)
                {
                    Clan c = ClanManager.getClan(p.clanId);
                    if (c._id > 0 && c._news != clan_news && (c.owner_id == _client.player_id || p.clanAccess >= 1 && p.clanAccess <= 2))
                    {
                        if (ComDiv.updateDB("clan_data", "clan_news", clan_news, "clan_id", c._id))
                            c._news = clan_news;
                        else
                            erro = 2147487859;
                    }
                    else erro = 2147487835;
                }
                else erro = 2147487835;
            }
            catch
            {
                erro = 2147487859;
            }
            _client.SendPacket(new CLAN_REPLACE_NOTICE_PAK(erro));
        }
    }
}
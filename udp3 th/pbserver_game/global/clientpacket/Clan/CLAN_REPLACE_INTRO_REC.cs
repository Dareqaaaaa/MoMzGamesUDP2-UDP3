using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class CLAN_REPLACE_INTRO_REC : ReceiveGamePacket
    {
        private string clan_info;
        private uint erro;
        public CLAN_REPLACE_INTRO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            clan_info = readS(readC());
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p != null)
                {
                    Clan c = ClanManager.getClan(p.clanId);
                    if (c._id > 0 && c._info != clan_info && (c.owner_id == _client.player_id || p.clanAccess >= 1 && p.clanAccess <= 2))
                    {
                        if (ComDiv.updateDB("clan_data", "clan_info", clan_info, "clan_id", c._id))
                            c._info = clan_info;
                        else
                            erro = 2147487860;
                    }
                    else erro = 2147487835;
                }
                else erro = 2147487835;
            }
            catch
            {
                erro = 2147487860;
            }
            _client.SendPacket(new CLAN_REPLACE_INTRO_PAK(erro));
        }
    }
}
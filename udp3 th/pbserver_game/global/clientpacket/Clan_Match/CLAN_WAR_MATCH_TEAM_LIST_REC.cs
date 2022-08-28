using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_MATCH_TEAM_LIST_REC : ReceiveGamePacket
    {
        private int page;
        public CLAN_WAR_MATCH_TEAM_LIST_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            page = readH();
            //Logger.warning("[Match_Team_List] Page: " + page);
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || p._match == null)
                    return;
                Channel ch = p.getChannel();
                if (ch != null && ch._type == 4)
                    _client.SendPacket(new CLAN_WAR_MATCH_TEAM_LIST_PAK(page, ch._matchs, p._match._matchId));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
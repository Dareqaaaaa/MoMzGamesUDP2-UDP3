using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_MATCH_TEAM_CONTEXT_REC : ReceiveGamePacket
    {
        public CLAN_WAR_MATCH_TEAM_CONTEXT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
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
                    _client.SendPacket(new CLAN_WAR_MATCH_TEAM_CONTEXT_PAK(ch._matchs.Count));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
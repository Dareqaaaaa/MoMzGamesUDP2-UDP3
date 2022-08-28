using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_PARTY_LIST_REC : ReceiveGamePacket
    {
        private List<Match> partyList = new List<Match>();
        private int page;
        public CLAN_WAR_PARTY_LIST_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            page = readC();
            //Logger.warning("[Party_List] Page: " + page);
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                if (p.clanId > 0)
                {
                    Channel ch = p.getChannel();
                    if (ch != null && ch._type == 4)
                    {
                        lock (ch._matchs)
                        {
                            foreach (Match m in ch._matchs)
                                if (m.clan._id == p.clanId)
                                    partyList.Add(m);
                        }
                    }
                }
                _client.SendPacket(new CLAN_WAR_PARTY_LIST_PAK(p.clanId == 0 ? 91 : 0, partyList));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
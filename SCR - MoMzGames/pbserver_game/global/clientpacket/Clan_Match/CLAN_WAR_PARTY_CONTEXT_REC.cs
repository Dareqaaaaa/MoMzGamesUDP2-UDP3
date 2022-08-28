using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_PARTY_CONTEXT_REC : ReceiveGamePacket
    {
        private int matchs;
        public CLAN_WAR_PARTY_CONTEXT_REC(GameClient client, byte[] data)
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
                if (p != null && p.clanId > 0)
                {
                    Channel ch = p.getChannel();
                    if (ch != null && ch._type == 4)
                    {
                        lock (ch._matchs)
                        {
                            foreach (Match m in ch._matchs)
                                if (m.clan._id == p.clanId)
                                    matchs++;
                        }
                    }
                }
                _client.SendPacket(new CLAN_WAR_PARTY_CONTEXT_PAK(matchs));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
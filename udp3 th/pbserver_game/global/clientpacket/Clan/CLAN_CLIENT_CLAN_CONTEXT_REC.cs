using Core;
using Game.data.managers;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_CLIENT_CLAN_CONTEXT_REC : ReceiveGamePacket
    {
        public CLAN_CLIENT_CLAN_CONTEXT_REC(GameClient client, byte[] data)
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
                 _client.SendPacket(new CLAN_CLIENT_CLAN_CONTEXT_PAK(ClanManager._clans.Count));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
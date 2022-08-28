using Core;
using Core.models.account.clan;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_GET_INFO_REC : ReceiveGamePacket
    {
        private int clanId, unk;
        public CLAN_GET_INFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            clanId = readD();
            unk = readC(); //1 = Sempre | 0 = Quando passa dono
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                Clan c = ClanManager.getClan(clanId);
                if (c._id > 0)
                    _client.SendPacket(new CLAN_DETAIL_INFO_PAK(1, c));
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_GET_INFO_REC: " + ex.ToString());
            }
        }
    }
}
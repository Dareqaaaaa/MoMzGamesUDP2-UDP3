﻿using Core.models.account.clan;
using Core.server;
using Game.data.managers;

namespace Game.data.sync.client_side
{
    public class Net_Clan_Servers_Sync
    {
        public static void Load(ReceiveGPacket p)
        {
            int type = p.readC();
            int clanId = p.readD();
            long ownerId;
            int date;
            string name, info;
            Clan clanCache = ClanManager.getClan(clanId);
            if (type == 0)
            {
                if (clanCache != null)
                    return;
                ownerId = p.readQ();
                date = p.readD();
                name = p.readS(p.readC());
                info = p.readS(p.readC());
                Clan clan = new Clan { _id = clanId, _name = name, owner_id = ownerId, _logo = 0, _info = info, creationDate = date };
                ClanManager.AddClan(clan);
            }
            else
            {
                if (clanCache != null)
                    ClanManager.RemoveClan(clanCache);
            }
        }
    }
}
using Core;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class CLAN_MEMBER_LIST_REC : ReceiveGamePacket
    {
        private int page;
        public CLAN_MEMBER_LIST_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            page = readC();
        }

        public override void run()
        {
            try
            {
                Account pl = _client._player;
                if (pl == null)
                    return;
                Clan clan = ClanManager.getClan(pl.clanId);
                if (clan._id == 0)
                {
                    _client.SendPacket(new CLAN_MEMBER_LIST_PAK(-1));
                    return;
                }
                List<Account> clanPlayers = ClanManager.getClanPlayers(pl.clanId, -1, false);
                using (SendGPacket p = new SendGPacket())
                {
                    int count = 0;
                    for (int i = (page * 14); i < clanPlayers.Count; i++)
                    {
                        WriteData(clanPlayers[i], p);
                        if (++count == 14)
                            break;
                    }
                    _client.SendPacket(new CLAN_MEMBER_LIST_PAK(page, count, p.mstream.ToArray()));
                }
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_MEMBER_LIST_REC: " + ex.ToString());
            }
        }
        private void WriteData(Account member, SendGPacket p)
        {
            p.writeQ(member.player_id);
            p.writeS(member.player_name, 33);
            p.writeC((byte)member._rank);
            p.writeC((byte)member.clanAccess);
            p.writeQ(ComDiv.GetClanStatus(member._status, member._isOnline));
            p.writeD(member.clanDate);
            p.writeC((byte)member.name_color);
        }
    }
}
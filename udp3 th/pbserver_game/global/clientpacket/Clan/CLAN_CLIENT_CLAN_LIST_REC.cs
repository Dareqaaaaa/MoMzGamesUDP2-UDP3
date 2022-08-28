using Core;
using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_CLIENT_CLAN_LIST_REC : ReceiveGamePacket
    {
        private uint page;
        public CLAN_CLIENT_CLAN_LIST_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            page = readUD();
        }

        public override void run()
        {
            try
            {
                if (_client == null || _client._player == null)
                    return;
                int count = 0;
                using (SendGPacket p = new SendGPacket())
                {
                    lock (ClanManager._clans)
                    {
                        for (int i = (int)page * 170; i < ClanManager._clans.Count; i++)
                        {
                            Clan clan = ClanManager._clans[i];
                            WriteData(clan, p);
                            if (++count == 170)
                                break;
                        }
                    }
                    _client.SendPacket(new CLAN_CLIENT_CLAN_LIST_PAK(page, count, p.mstream.ToArray()));
                }
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
        private void WriteData(Clan clan, SendGPacket p)
        {
            p.writeD(clan._id);
            p.writeS(clan._name, 17);
            p.writeC((byte)clan._rank);
            p.writeC((byte)PlayerManager.getClanPlayers(clan._id));
            p.writeC((byte)clan.maxPlayers);
            p.writeD(clan.creationDate);
            p.writeD(clan._logo);
            p.writeC((byte)clan._name_color);
        }
    }
}
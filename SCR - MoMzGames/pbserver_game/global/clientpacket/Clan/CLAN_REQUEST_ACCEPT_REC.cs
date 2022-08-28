/*
 * Arquivo: CLAN_REQUEST_ACCEPT_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 17/09/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.managers;
using Core.models.account;
using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class CLAN_REQUEST_ACCEPT_REC : ReceiveGamePacket
    {
        private int result;
        public CLAN_REQUEST_ACCEPT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            Account p = _client._player;
            if (p == null)
                return;
            Clan clan = ClanManager.getClan(p.clanId);
            if (clan._id > 0 && (p.clanAccess >= 1 && p.clanAccess <= 2 || p.player_id == clan.owner_id))
            {
                List<Account> clanPlayers = ClanManager.getClanPlayers(clan._id, -1, true);
                if (clanPlayers.Count >= clan.maxPlayers)
                {
                    result = -1;
                    return;
                }
                int playersCount = readC();
                for (int i = 0; i < playersCount; i++)
                {
                    Account pl = AccountManager.getAccount(readQ(), 0);
                    if (pl != null && clanPlayers.Count < clan.maxPlayers && pl.clanId == 0 && PlayerManager.getRequestClanId(pl.player_id) > 0)
                    {
                        using (CLAN_MEMBER_INFO_INSERT_PAK packet = new CLAN_MEMBER_INFO_INSERT_PAK(pl))
                            ClanManager.SendPacket(packet, clanPlayers);
                        pl.clanId = p.clanId;
                        pl.clanDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                        pl.clanAccess = 3;
                        SEND_CLAN_INFOS.Load(pl, null, 3);

                        ComDiv.updateDB("accounts", "player_id", pl.player_id, new string[] { "clanaccess", "clan_id", "clandate" }, pl.clanAccess, pl.clanId, pl.clanDate);

                        PlayerManager.DeleteInviteDb(p.clanId, pl.player_id);
                        if (pl._isOnline)
                        {
                            pl.SendPacket(new CLAN_GET_CLAN_MEMBERS_PAK(clanPlayers), false);
                            Room r = pl._room;
                            if (r != null)
                                r.SendPacketToPlayers(new ROOM_GET_SLOTONEINFO_PAK(pl, clan));
                            pl.SendPacket(new CLAN_NEW_INFOS_PAK(clan, clanPlayers.Count + 1), false);
                        }
                        if (MessageManager.getMsgsCount(pl.player_id) < 100)
                        {
                            Message msg = CreateMessage(clan, pl.player_id, _client.player_id);
                            if (msg != null && pl._isOnline)
                                pl.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                        }
                        result++;
                        clanPlayers.Add(pl);
                    }
                }
                clanPlayers = null;
            }
            else result = -1;
        }

        public override void run()
        {
            try
            {
                _client.SendPacket(new CLAN_REQUEST_ACCEPT_PAK((uint)result));
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_INVITE_ACCEPT_REC " + ex.ToString());
            }
        }
        private Message CreateMessage(Clan clan, long owner, long senderId)
        {
            Message msg = new Message(15)
            {
                sender_name = clan._name,
                sender_id = senderId,
                clanId = clan._id,
                type = 4,
                state = 1,
                cB = NoteMessageClan.InviteAccept
            };
            return MessageManager.CreateMessage(owner, msg) ? msg : null;
        }
    }
}
﻿/*
 * Arquivo: CLAN_MESSAGE_REQUEST_ACCEPT_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 02/06/2017
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
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class CLAN_MESSAGE_REQUEST_INTERACT_REC : ReceiveGamePacket
    {
        private int clanId, type;
        public CLAN_MESSAGE_REQUEST_INTERACT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            clanId = readD();
            readD();
            type = readC();
        }

        public override void run()
        {
            Account player = _client._player;
            if (player == null || player.player_name.Length == 0)
                return;
            Clan clan = ClanManager.getClan(clanId);
            List<Account> clanPlayers = ClanManager.getClanPlayers(clanId, -1, true);
            if (clan._id == 0)
                _client.SendPacket(new CLAN_MESSAGE_REQUEST_ACCEPT_PAK(2147487835));
            else if (player.clanId > 0)
                _client.SendPacket(new CLAN_MESSAGE_REQUEST_ACCEPT_PAK(2147487832));
            else if (clan.maxPlayers <= clanPlayers.Count)
                _client.SendPacket(new CLAN_MESSAGE_REQUEST_ACCEPT_PAK(2147487830));
            else if (type == 0 || type == 1)
            {
                try
                {
                    uint erro = 0;
                    Account master = AccountManager.getAccount(clan.owner_id, 0);
                    if (master != null)
                    {
                        if (MessageManager.getMsgsCount(clan.owner_id) < 100)
                        {
                            Message msg = CreateMessage(clan, player.player_name, _client.player_id);
                            if (msg != null && master._isOnline)
                                master.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                        }
                        if (type == 1)
                        {
                            int date = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                            if (ComDiv.updateDB("accounts", "player_id", player.player_id, new string[] { "clan_id", "clanaccess", "clandate" }, clan._id, 3, date))
                            {
                                using (CLAN_MEMBER_INFO_INSERT_PAK packet = new CLAN_MEMBER_INFO_INSERT_PAK(player))
                                    ClanManager.SendPacket(packet, clanPlayers);
                                player.clanId = clan._id;
                                player.clanDate = date;
                                player.clanAccess = 3;
                                _client.SendPacket(new CLAN_GET_CLAN_MEMBERS_PAK(clanPlayers));
                                Room room = player._room;
                                if (room != null)
                                    room.SendPacketToPlayers(new ROOM_GET_SLOTONEINFO_PAK(player, clan));
                                _client.SendPacket(new CLAN_NEW_INFOS_PAK(clan, master, clanPlayers.Count + 1));
                            }
                            else erro = 0x80000000;
                        }
                    }
                    else erro = 0x80000000;
                    _client.SendPacket(new BOX_MESSAGE_SEND_PAK(erro));
                }
                catch (Exception ex)
                {
                    Logger.error(ex.ToString());
                }
            }
        }
        private Message CreateMessage(Clan clan, string player, long senderId)
        {
            Message msg = new Message(15)
            {
                sender_name = clan._name,
                sender_id = senderId,
                clanId = clan._id,
                type = 4,
                text = player,
                state = 1,
                cB = type == 0 ? NoteMessageClan.JoinDenial : NoteMessageClan.JoinAccept
            };
            return MessageManager.CreateMessage(clan.owner_id, msg) ? msg : null;
        }
    }
}
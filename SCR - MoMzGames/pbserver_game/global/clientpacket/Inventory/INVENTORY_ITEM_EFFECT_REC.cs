using Core;
using Core.managers;
using Core.models.account.clan;
using Core.models.account.players;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class INVENTORY_ITEM_EFFECT_REC : ReceiveGamePacket
    {
        private long objId;
        private uint objetivo, erro = 1;
        private byte[] info;
        private string txt;
        public INVENTORY_ITEM_EFFECT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            objId = readQ();
            info = readB(readC());
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                ItemsModel item = p == null ? null : p._inventory.getItem(objId);
                if (item != null && item._id > 1300000000)
                {
                    int cuponId = ComDiv.createItemId(12, ComDiv.getIdStatics(item._id, 2), ComDiv.getIdStatics(item._id, 3), 0);
                    uint cuponDays = uint.Parse(DateTime.Now.AddDays(ComDiv.getIdStatics(item._id, 4)).ToString("yyMMddHHmm"));

                    if (cuponId == 1201047000 || cuponId == 1201051000 || cuponId == 1200010000)
                        txt = ComDiv.arrayToString(info, info.Length);
                    else if (cuponId == 1201052000 || cuponId == 1200005000)
                        objetivo = BitConverter.ToUInt32(info, 0);
                    else if (info.Length > 0)
                        objetivo = info[0];

                    CreateCuponEffects(cuponId, cuponDays, p);
                }
                else erro = 0x80000000;
                _client.SendPacket(new INVENTORY_ITEM_EQUIP_PAK(erro, item, p));
            }
            catch (Exception ex)
            {
                Logger.info("INVENTORY_ITEM_EFFECT_REC: " + ex.ToString());
                _client.SendPacket(new INVENTORY_ITEM_EQUIP_PAK(0x80000000));
            }
        }
        /// <summary>
        /// Gera efeitos dos cupons na Database.
        /// </summary>
        /// <param name="cuponId">Id do cupom</param>
        /// <param name="cuponDays">Dias do cupom</param>
        /// <param name="p">Jogador</param>
        private void CreateCuponEffects(int cupomId, uint cuponDays, Account p)
        {
            if (cupomId == 1201051000)
            {
                if (string.IsNullOrEmpty(txt) || txt.Length > 16)
                    erro = 0x80000000;
                else
                {
                    Clan c = ClanManager.getClan(p.clanId);
                    if (c._id > 0 && c.owner_id == _client.player_id)
                    {
                        if (!ClanManager.isClanNameExist(txt) && ComDiv.updateDB("clan_data", "clan_name", txt, "clan_id", p.clanId))
                        {
                            c._name = txt;
                            using (CLAN_CHANGE_NAME_PAK packet = new CLAN_CHANGE_NAME_PAK(txt))
                                ClanManager.SendPacket(packet, p.clanId, -1, true, true);
                        }
                        else erro = 0x80000000;
                    }
                    else erro = 0x80000000;
                }
            }
            else if (cupomId == 1201052000)
            {
                Clan clan = ClanManager.getClan(p.clanId);
                if (clan._id > 0 && clan.owner_id == _client.player_id && !ClanManager.isClanLogoExist((uint)objetivo) && 
                    PlayerManager.updateClanLogo(p.clanId, (uint)objetivo))
                {
                    clan._logo = (uint)objetivo;
                    using (CLAN_CHANGE_LOGO_PAK packet = new CLAN_CHANGE_LOGO_PAK((uint)objetivo))
                        ClanManager.SendPacket(packet, p.clanId, -1, true, true);
                }
                else erro = 0x80000000;
            }
            else if (cupomId == 1201047000)
            {
                if (string.IsNullOrEmpty(txt) || txt.Length < ConfigGS.minNickSize || txt.Length > ConfigGS.maxNickSize ||
                    p._inventory.getItem(1200010000) != null)
                    erro = 0x80000000;
                else if (!PlayerManager.isPlayerNameExist(txt))
                {
                    if (ComDiv.updateDB("accounts", "player_name", txt, "player_id", p.player_id))
                    {
                        NickHistoryManager.CreateHistory(p.player_id, p.player_name, txt, "Change nick");
                        p.player_name = txt;
                        if (p._room != null)
                            using (ROOM_GET_NICKNAME_PAK packet = new ROOM_GET_NICKNAME_PAK(p._slotId, p.player_name, p.name_color))
                                p._room.SendPacketToPlayers(packet);
                        _client.SendPacket(new AUTH_CHANGE_NICKNAME_PAK(p.player_name));
                        if (p.clanId > 0)
                        {
                            using (CLAN_MEMBER_INFO_UPDATE_PAK packet = new CLAN_MEMBER_INFO_UPDATE_PAK(p))
                                ClanManager.SendPacket(packet, p.clanId, -1, true, true);
                        }
                        AllUtils.syncPlayerToFriends(p, true);
                    }
                    else erro = 0x80000000;
                }
                else erro = 2147483923;
            }
            else if (cupomId == 1200006000)
            {
                if (ComDiv.updateDB("accounts", "name_color", (int)objetivo, "player_id", p.player_id))
                {
                    p.name_color = (int)objetivo;
                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "NameColor [Active]", 2, cuponDays, 0)));
                    _client.SendPacket(new BASE_2612_PAK(p));
                    if (p._room != null)
                        using (ROOM_GET_NICKNAME_PAK packet = new ROOM_GET_NICKNAME_PAK(p._slotId, p.player_name, p.name_color))
                            p._room.SendPacketToPlayers(packet);
                }
                else erro = 0x80000000;
            }
            else if (cupomId == 1200009000)
            {
                if ((int)objetivo >= 51 || (int)objetivo < p._rank - 10 || (int)objetivo > p._rank + 10)
                    erro = 0x80000000;
                else if (ComDiv.updateDB("player_bonus", "fakerank", (int)objetivo, "player_id", p.player_id))
                {
                    p._bonus.fakeRank = (int)objetivo;
                    _client.SendPacket(new BASE_USER_EFFECTS_PAK(info.Length, p._bonus));
                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "Patente falsa [Active]", 2, cuponDays, 0)));
                    if (p._room != null)
                        p._room.updateSlotsInfo();
                }
                else erro = 0x80000000;
            }
            else if (cupomId == 1200010000)
            {
                if (string.IsNullOrEmpty(txt) || txt.Length < ConfigGS.minNickSize || txt.Length > ConfigGS.maxNickSize)
                    erro = 0x80000000;
                else if (ComDiv.updateDB("player_bonus", "fakenick", p.player_name, "player_id", p.player_id) &&
                    ComDiv.updateDB("accounts", "player_name", txt, "player_id", p.player_id))
                {
                    p._bonus.fakeNick = p.player_name;
                    p.player_name = txt;
                    _client.SendPacket(new BASE_USER_EFFECTS_PAK(info.Length, p._bonus));
                    _client.SendPacket(new AUTH_CHANGE_NICKNAME_PAK(p.player_name));
                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "FakeNick [Active]", 2, cuponDays, 0)));
                    if (p._room != null)
                        p._room.updateSlotsInfo();
                }
                else erro = 0x80000000;
            }
            else if (cupomId == 1200014000)
            {
                if (ComDiv.updateDB("player_bonus", "sightcolor", (int)objetivo, "player_id", p.player_id))
                {
                    p._bonus.sightColor = (int)objetivo;
                    _client.SendPacket(new BASE_USER_EFFECTS_PAK(info.Length, p._bonus));
                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, "Cor da mira [Active]", 2, cuponDays, 0)));
                }
                else erro = 0x80000000;
            }
            else if (cupomId == 1200005000)
            {
                Clan c = ClanManager.getClan(p.clanId);
                if (c._id > 0 && c.owner_id == _client.player_id && ComDiv.updateDB("clan_data", "color", (int)objetivo, "clan_id", c._id))
                {
                    c._name_color = (int)objetivo;
                    _client.SendPacket(new CLAN_CHANGE_NAME_COLOR_PAK((byte)c._name_color));
                }
                else erro = 0x80000000;
            }
            else if (cupomId == 1201085000)
            {
                if (p._room != null)
                {
                    Account pR = p._room.getPlayerBySlot((int)objetivo);
                    if (pR != null)
                        _client.SendPacket(new ROOM_INSPECTPLAYER_PAK(pR));
                    else erro = 0x80000000;
                }
                else erro = 0x80000000;
            }
            else
            {
                Logger.error("[ITEM_EFFECT] Efeito do cupom não encontrado! Id: " + cupomId);
                erro = 0x80000000;
            }
        }
    }
}
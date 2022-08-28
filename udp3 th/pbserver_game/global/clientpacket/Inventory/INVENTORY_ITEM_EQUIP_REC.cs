using Core;
using Core.managers;
using Core.models.account.clan;
using Core.models.account.players;
using Core.models.enums.flags;
using Core.models.randombox;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Game.global.clientpacket
{
    public class INVENTORY_ITEM_EQUIP_REC : ReceiveGamePacket
    {
        private long objId;
        private int itemId;
        private uint erro = 1, oldCOUNT;
        public INVENTORY_ITEM_EQUIP_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            objId = readQ();
        }

        public override void run()
        {
            if (_client == null || _client._player == null)
                return;
            try
            {
                Account p = _client._player;
                ItemsModel itemObj = p._inventory.getItem(objId);
                if (itemObj != null)
                {
                    itemId = itemObj._id;
                    oldCOUNT = itemObj._count;
                    if (itemObj._category == 3 && p._inventory._items.Count >= 500)
                    {
                        _client.SendPacket(new INVENTORY_ITEM_EQUIP_PAK(0x80001029));
                        Logger.warning("O inventário do jogador: '" + p.player_name + "' está cheio demais!");
                        return;
                    }
                    if (itemId == 1301049000)
                    {
                        if (PlayerManager.updateKD(p.player_id, 0, 0, p._statistic.headshots_count, p._statistic.totalkills_count))
                        {
                            p._statistic.kills_count = 0;
                            p._statistic.deaths_count = 0;
                            _client.SendPacket(new BASE_USER_CHANGE_STATS_PAK(p._statistic));
                        }
                        else erro = 0x80000000;
                    }
                    else if (itemId == 1301048000)
                    {
                        if (PlayerManager.updateFights(0, 0, 0, 0, p._statistic.totalfights_count, p.player_id))
                        {
                            p._statistic.fights = 0;
                            p._statistic.fights_win = 0;
                            p._statistic.fights_lost = 0;
                            p._statistic.fights_draw = 0;
                            _client.SendPacket(new BASE_USER_CHANGE_STATS_PAK(p._statistic));
                        }
                        else erro = 0x80000000;
                    }
                    else if (itemId == 1301050000)
                    {
                        if (ComDiv.updateDB("accounts", "escapes", 0, "player_id", p.player_id))
                        {
                            p._statistic.escapes = 0;
                            _client.SendPacket(new BASE_USER_CHANGE_STATS_PAK(p._statistic));
                        }
                        else erro = 0x80000000;
                    }
                    else if (itemId == 1301053000)
                    {
                        if (PlayerManager.updateClanBattles(p.clanId, 0, 0, 0))
                        {
                            Clan c = ClanManager.getClan(p.clanId);
                            if (c._id > 0 && c.owner_id == _client.player_id)
                            {
                                c.partidas = 0;
                                c.vitorias = 0;
                                c.derrotas = 0;
                                _client.SendPacket(new CLAN_CHANGE_FIGHTS_PAK());
                            }
                            else erro = 0x80000000;
                        }
                        else erro = 0x80000000;
                    }
                    else if (itemId == 1301055000)
                    {
                        Clan c = ClanManager.getClan(p.clanId);
                        if (c._id > 0 && c.owner_id == _client.player_id)
                        {
                            if (c.maxPlayers + 50 <= 250 && ComDiv.updateDB("clan_data", "max_players", c.maxPlayers + 50, "clan_id", p.clanId))
                            {
                                c.maxPlayers += 50;
                                _client.SendPacket(new CLAN_CHANGE_MAX_PLAYERS_PAK(c.maxPlayers));
                            }
                            else erro = 0x80001056;
                        }
                        else erro = 0x80001056;
                    }
                    else if (itemId == 1301056000)
                    {
                        Clan c = ClanManager.getClan(p.clanId);
                        if (c._id > 0 && c._pontos != 1000)
                        {
                            if (ComDiv.updateDB("clan_data", "pontos", 1000.0f, "clan_id", p.clanId))
                            {
                                c._pontos = 1000;
                                _client.SendPacket(new CLAN_CHANGE_POINTS_PAK());
                            }
                            else erro = 0x80001056;
                        }
                        else erro = 0x80001056;
                    }
                    else if (itemId > 1301113000 && itemId < 1301119000)
                    {
                        int goldReceive = itemId == 1301114000 ? 500 : (itemId == 1301115000 ? 1000 : (itemId == 1301116000 ? 5000 : (itemId == 1301117000 ? 10000 : 30000)));
                        if (ComDiv.updateDB("accounts", "gp", p._gp + goldReceive, "player_id", p.player_id))
                        {
                            p._gp += goldReceive;
                            _client.SendPacket(new AUTH_GOLD_REWARD_PAK(goldReceive, p._gp, 0));
                        }
                        else erro = 0x80000000;
                    }
                    else if (itemId == 1301999000)
                    {
                        if (ComDiv.updateDB("accounts", "exp", p._exp + 515999, "player_id", p.player_id))
                        {
                            p._exp += 515999;
                            _client.SendPacket(new A_3096_PAK(515999, 0));
                        }
                        else erro = 0x80000000;
                    }
                    else if (itemObj._category == 3 && RandomBoxXML.ContainsBox(itemId))
                    {
                        RandomBoxModel box = RandomBoxXML.getBox(itemId);
                        if (box != null)
                        {
                            List<RandomBoxItem> sortedList = box.getSortedList(GetRandomNumber(1, 100));
                            List<RandomBoxItem> rewardList = box.getRewardList(sortedList, GetRandomNumber(0, sortedList.Count));
                            if (rewardList.Count > 0)
                            {
                                int itemIdx = rewardList[0].index;
                                _client.SendPacket(new AUTH_RANDOM_BOX_REWARD_PAK(itemId, itemIdx));
                                List<ItemsModel> rewards = new List<ItemsModel>();
                                for (int i = 0; i < rewardList.Count; i++)
                                {
                                    RandomBoxItem cupom = rewardList[i];
                                    if (cupom.item != null)
                                        rewards.Add(cupom.item);
                                    else if (PlayerManager.updateAccountGold(p.player_id, p._gp + (int)cupom.count))
                                    {
                                        p._gp += (int)cupom.count;
                                        _client.SendPacket(new AUTH_GOLD_REWARD_PAK((int)cupom.count, p._gp, 0));
                                    }
                                    else
                                    {
                                        erro = 0x80000000;
                                        break;
                                    }
                                    if (cupom.special)
                                    {
                                        using (AUTH_JACKPOT_NOTICE_PAK packet = new AUTH_JACKPOT_NOTICE_PAK(p.player_name, itemId, itemIdx))
                                            GameManager.SendPacketToAllClients(packet);
                                    }
                                }
                                if (rewards.Count > 0)
                                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, rewards));
                            }
                            else erro = 0x80000000;
                            sortedList = null;
                            rewardList = null;
                        }
                        else erro = 0x80000000;
                    }
                    else
                    {
                        int wclass = ComDiv.getIdStatics(itemObj._id, 1);
                        if (wclass <= 11)
                        {
                            if (itemObj._equip == 1)
                            {
                                itemObj._equip = 2;
                                itemObj._count = uint.Parse(DateTime.Now.AddSeconds(itemObj._count).ToString("yyMMddHHmm"));
                                ComDiv.updateDB("player_items", "object_id", objId, "owner_id", p.player_id, new string[] { "count", "equip" }, (long)itemObj._count, itemObj._equip);
                            }
                            else erro = 0x80000000;
                        }
                        else if (wclass == 13)
                            cupomIncreaseDays(p, itemObj._name);
                        else if (wclass == 15)
                            cupomIncreaseGold(p, itemObj._id);
                        else erro = 0x80000000;
                    }
                }
                else erro = 0x80000000;
                _client.SendPacket(new INVENTORY_ITEM_EQUIP_PAK(erro, itemObj, p));
            }
            catch (OverflowException ex)
            {
                Logger.error("[Data do erro: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "] Obj: " + objId + "; Id do item: " + itemId + "; Count na Db: " + oldCOUNT + "; Id do cara: " + _client.player_id + "; Nick: '" + _client._player.player_name + "'\r\n" + ex.ToString());
                _client.SendPacket(new INVENTORY_ITEM_EQUIP_PAK(0x80000000));
                }
            catch (Exception ex)
            {
                Logger.info("INVENTORY_ITEM_EQUIP_REC: " + ex.ToString());
                _client.SendPacket(new INVENTORY_ITEM_EQUIP_PAK(0x80000000));
            }
        }
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        private static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }
        private void cupomIncreaseDays(Account p, string originalName)
        {
            int cupomId = ComDiv.createItemId(12, ComDiv.getIdStatics(itemId, 2), ComDiv.getIdStatics(itemId, 3), 0),
                days = ComDiv.getIdStatics(itemId, 4);
            CupomEffects eff = p.effects;
            if (cupomId == 1200065000 && ((eff & (CupomEffects.Colete5 | CupomEffects.Colete10 | CupomEffects.Colete20)) > 0) ||
                cupomId == 1200079000 && ((eff & (CupomEffects.Colete5 | CupomEffects.Colete10 | CupomEffects.Colete90)) > 0) ||
                cupomId == 1200044000 && ((eff & (CupomEffects.Colete5 | CupomEffects.Colete20 | CupomEffects.Colete90)) > 0) ||
                cupomId == 1200030000 && ((eff & (CupomEffects.Colete20 | CupomEffects.Colete10 | CupomEffects.Colete90)) > 0) ||
                cupomId == 1200078000 && ((eff & (CupomEffects.IronBullet | CupomEffects.HollowPoint | CupomEffects.HollowPointF)) > 0) ||
                cupomId == 1200032000 && ((eff & (CupomEffects.HollowPointF | CupomEffects.HollowPointPlus | CupomEffects.IronBullet)) > 0) ||
                cupomId == 1200031000 && ((eff & (CupomEffects.HollowPointF | CupomEffects.HollowPointPlus | CupomEffects.HollowPoint)) > 0) ||
                cupomId == 1200036000 && ((eff & (CupomEffects.HollowPoint | CupomEffects.HollowPointPlus | CupomEffects.IronBullet)) > 0) ||
                cupomId == 1200028000 && eff.HasFlag(CupomEffects.HP5) ||
                cupomId == 1200040000 && eff.HasFlag(CupomEffects.HP10))
            {
                erro = 0x80000000;
            }
            else
            {
                ItemsModel item = p._inventory.getItem(cupomId);
                if (item == null)
                {
                    bool changed = p._bonus.AddBonuses(cupomId);
                    CupomFlag cupom = CupomEffectManager.getCupomEffect(cupomId);
                    if (cupom != null && cupom.EffectFlag > 0 && !p.effects.HasFlag(cupom.EffectFlag))
                    {
                        p.effects |= cupom.EffectFlag;
                        PlayerManager.updateCupomEffects(p.player_id, p.effects);
                    }
                    if (changed)
                        PlayerManager.updatePlayerBonus(p.player_id, p._bonus.bonuses, p._bonus.freepass);
                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, p, new ItemsModel(cupomId, 3, originalName + " [Active]", 2, uint.Parse(DateTime.Now.AddDays(days).ToString("yyMMddHHmm")))));
                }
                else
                {
                    DateTime data = DateTime.ParseExact(item._count.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture);
                    item._count = uint.Parse(data.AddDays(days).ToString("yyMMddHHmm"));
                    ComDiv.updateDB("player_items", "count", (long)item._count, "object_id", item._objId, "owner_id", p.player_id);
                    _client.SendPacket(new INVENTORY_ITEM_CREATE_PAK(2, p, item));
                }
            }
        }
        private void cupomIncreaseGold(Account p, int cupomId)
        {
            int gold = ComDiv.getIdStatics(cupomId, 4) * 1000;
            gold += ComDiv.getIdStatics(cupomId, 3) * 100;
            gold += ComDiv.getIdStatics(cupomId, 2) * 1000000;
            if (PlayerManager.updateAccountGold(p.player_id, p._gp + (gold)))
            {
                p._gp += gold;
                _client.SendPacket(new AUTH_GOLD_REWARD_PAK(gold, p._gp, 0));
            }
            else
                erro = 0x80000000;
        }
    }
}
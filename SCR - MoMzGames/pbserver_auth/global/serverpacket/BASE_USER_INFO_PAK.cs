/*
 * Arquivo: BASE_USER_INFO_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 01/07/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.configs;
using Auth.data.managers;
using Auth.data.model;
using Core.managers;
using Core.managers.events;
using Core.managers.server;
using Core.models.account;
using Core.models.account.clan;
using Core.models.account.players;
using Core.server;
using Core.xml;
using System;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
    public class BASE_USER_INFO_PAK : SendPacket
    {
        private Account c;
        private Clan clan;
        private uint _erro;
        private bool _xmas;
        private List<ItemsModel> charas = new List<ItemsModel>(),
            weapons = new List<ItemsModel>(),
            cupons = new List<ItemsModel>();
        public BASE_USER_INFO_PAK(Account p)
        {
            c = p;
            if (c != null && c._inventory._items.Count == 0)
            {
                clan = ClanManager.getClanDB(c.clan_id, 1);
                c.LoadInventory();
                c.LoadMissionList();
                c.DiscountPlayerItems();
                GetInventoryInfo();
            }
            else _erro = 0x80000000;
        }
        private void GetInventoryInfo()
        {
            lock (c._inventory._items)
            {
                for (int i = 0; i < c._inventory._items.Count; i++)
                {
                    ItemsModel item = c._inventory._items[i];
                    if (item._category == 1)
                        weapons.Add(item);
                    else if (item._category == 2)
                        charas.Add(item);
                    else if (item._category == 3)
                        cupons.Add(item);
                }
            }
        }
        private void CheckGameEvents(EventVisitModel evVisit)
        {
            long dateNow = long.Parse(DateTime.Now.ToString("yyMMddHHmm"));
            PlayerEvent pev = c._event;
            if (pev != null)
            {
                QuestModel evQuest = EventQuestSyncer.getRunningEvent();
                if (evQuest != null)
                {
                    long date = pev.LastQuestDate, finish = pev.LastQuestFinish;
                    if (pev.LastQuestDate < evQuest.startDate)
                    {
                        pev.LastQuestDate = 0;
                        pev.LastQuestFinish = 0;
                        c.SendPacket(new SERVER_MESSAGE_EVENT_QUEST_PAK());
                    }
                    if (pev.LastQuestFinish == 0)
                    {
                        c._mission.mission4 = 13; //MissionId
                        if (pev.LastQuestDate == 0)
                            pev.LastQuestDate = (uint)dateNow;
                    }
                    if (pev.LastQuestDate != date || pev.LastQuestFinish != finish)
                        EventQuestSyncer.ResetPlayerEvent(c.player_id, pev);
                }
                EventLoginModel evLogin = EventLoginSyncer.getRunningEvent();
                if (evLogin != null && !(evLogin.startDate < pev.LastLoginDate && pev.LastLoginDate < evLogin.endDate))
                {
                    ItemsModel item = new ItemsModel(evLogin._rewardId, evLogin._category, "Login event", 1, (uint)evLogin._count);
                    PlayerManager.tryCreateItem(item, c._inventory, c.player_id);
                    c.SendPacket(new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0));
                    if (item._category == 1)
                        weapons.Add(item);
                    else if (item._category == 2)
                        charas.Add(item);
                    else if (item._category == 3)
                        cupons.Add(item);
                    ComDiv.updateDB("player_events", "last_login_date", dateNow, "player_id", c.player_id);
                }
                if (evVisit != null && pev.LastVisitEventId != evVisit.id)
                {
                    pev.LastVisitEventId = evVisit.id;
                    pev.LastVisitSequence1 = 0;
                    pev.LastVisitSequence2 = 0;
                    pev.NextVisitDate = 0;
                    EventVisitSyncer.ResetPlayerEvent(c.player_id, evVisit.id);
                }
                EventXmasModel evXmas = EventXmasSyncer.getRunningEvent();
                if (evXmas != null)
                {
                    if (pev.LastXmasRewardDate < evXmas.startDate)
                    {
                        pev.LastXmasRewardDate = 0;
                        ComDiv.updateDB("player_events", "last_xmas_reward_date", 0, "player_id", c.player_id);
                    }
                    if (!(pev.LastXmasRewardDate > evXmas.startDate && pev.LastXmasRewardDate <= evXmas.endDate))
                        _xmas = true;
                }
            }
            ComDiv.updateDB("accounts", "last_login", dateNow, "player_id", c.player_id);
        }
        public override void write()
        {
            writeH(2566);
            writeD(_erro);
            if (_erro != 0)
                return;
            ServerConfig cfg = LoginManager.Config;
            EventVisitModel evVisit = EventVisitSyncer.getRunningEvent();
            writeC(0);
            writeS(c.player_name, 33);
            writeD(c._exp);
            writeD(c._rank);
            writeD(c._rank);
            writeD(c._gp);
            writeD(c._money);
            writeD(clan._id);
            writeD(c.clanAccess);
            writeQ(0);
            writeC((byte)c.pc_cafe);
            writeC((byte)c.tourneyLevel);
            writeC((byte)c.name_color);
            writeS(clan._name, 17);
            writeC((byte)clan._rank);
            writeC((byte)clan.getClanUnit());
            writeD(clan._logo);
            writeC((byte)clan._name_color);
            writeD(10000);
            writeC(0);
            writeD(0);
            writeD(c.LastRankUpDate); //109 BYTES
            writeD(c._statistic.fights);
            writeD(c._statistic.fights_win);
            writeD(c._statistic.fights_lost);
            writeD(c._statistic.fights_draw);
            writeD(c._statistic.kills_count);
            writeD(c._statistic.headshots_count);
            writeD(c._statistic.deaths_count);
            writeD(c._statistic.totalfights_count);
            writeD(c._statistic.totalkills_count);
            writeD(c._statistic.escapes);
            writeD(c._statistic.fights);
            writeD(c._statistic.fights_win);
            writeD(c._statistic.fights_lost);
            writeD(c._statistic.fights_draw);
            writeD(c._statistic.kills_count);
            writeD(c._statistic.headshots_count);
            writeD(c._statistic.deaths_count);
            writeD(c._statistic.totalfights_count);
            writeD(c._statistic.totalkills_count);
            writeD(c._statistic.escapes);
            writeD(c._equip._red);
            writeD(c._equip._blue);
            writeD(c._equip._helmet);
            writeD(c._equip._beret);
            writeD(c._equip._dino);
            writeD(c._equip._primary);
            writeD(c._equip._secondary);
            writeD(c._equip._melee);
            writeD(c._equip._grenade);
            writeD(c._equip._special);
            writeH(0);
            writeD(c._bonus.fakeRank);
            writeD(c._bonus.fakeRank);
            writeS(c._bonus.fakeNick, 33);
            writeH((short)c._bonus.sightColor);
            writeC(31); //pais?
            CheckGameEvents(evVisit);
            if (cfg.ClientVersion == "1.15.37")
            {
                writeC(1); //inventário?
                writeD(charas.Count);
                writeD(weapons.Count);
                writeD(cupons.Count);
                writeD(0);
                for (int i = 0; i < charas.Count; i++)
                {
                    ItemsModel item = charas[i];
                    writeQ(item._objId);
                    writeD(item._id);
                    writeC((byte)item._equip);
                    writeD(item._count);
                }
                for (int i = 0; i < weapons.Count; i++)
                {
                    ItemsModel item = weapons[i];
                    writeQ(item._objId);
                    writeD(item._id);
                    writeC((byte)item._equip);
                    writeD(item._count);
                }
                for (int i = 0; i < cupons.Count; i++)
                {
                    ItemsModel item = cupons[i];
                    writeQ(item._objId);
                    writeD(item._id);
                    writeC((byte)item._equip);
                    writeD(item._count);
                }
            }
            writeC(ConfigGA.Outpost);
            writeD(c.brooch);
            writeD(c.insignia);
            writeD(c.medal);
            writeD(c.blue_order);
            writeC((byte)c._mission.actualMission);
            writeC((byte)c._mission.card1);
            writeC((byte)c._mission.card2);
            writeC((byte)c._mission.card3);
            writeC((byte)c._mission.card4);
            writeB(ComDiv.getCardFlags(c._mission.mission1, c._mission.list1));
            writeB(ComDiv.getCardFlags(c._mission.mission2, c._mission.list2));
            writeB(ComDiv.getCardFlags(c._mission.mission3, c._mission.list3));
            writeB(ComDiv.getCardFlags(c._mission.mission4, c._mission.list4));
            writeC((byte)c._mission.mission1);
            writeC((byte)c._mission.mission2);
            writeC((byte)c._mission.mission3);
            writeC((byte)c._mission.mission4);
            writeB(c._mission.list1);
            writeB(c._mission.list2);
            writeB(c._mission.list3);
            writeB(c._mission.list4);
            writeQ(c._titles.Flags);
            writeC((byte)c._titles.Equiped1);
            writeC((byte)c._titles.Equiped2);
            writeC((byte)c._titles.Equiped3);
            writeD(c._titles.Slots);
            writeD(ConfigMaps.Tutorial);
            writeD(ConfigMaps.Deathmatch);
            writeD(ConfigMaps.Destruction);
            writeD(ConfigMaps.Sabotage);
            writeD(ConfigMaps.Supression);
            writeD(ConfigMaps.Defense);
            writeD(ConfigMaps.Challenge);
            writeD(ConfigMaps.Dinosaur);
            writeD(ConfigMaps.Sniper);
            writeD(ConfigMaps.Shotgun);
            writeD(ConfigMaps.HeadHunter);
            writeD(ConfigMaps.Knuckle);
            writeD(ConfigMaps.CrossCounter);
            writeD(ConfigMaps.Chaos);
            if (cfg.ClientVersion == "1.15.38" || cfg.ClientVersion == "1.15.39" || cfg.ClientVersion == "1.15.41" || cfg.ClientVersion == "1.15.42")
                writeD(ConfigMaps.TheifMode);

            writeC((byte)MapsXML.ModeList.Count);//124 maps ver 42

            writeC(4); //(Flag pages | 4 bytes)
            writeD(MapsXML.maps1);
            writeD(MapsXML.maps2);
            writeD(MapsXML.maps3);
            writeD(MapsXML.maps4); 
             foreach (ushort mode in MapsXML.ModeList)
              writeH(mode);// 2 byts por cartão
            writeB(MapsXML.TagList.ToArray());

            writeC(cfg.missions); //Pages Count
            writeD(MissionsXML._missionPage1);
            writeD(0); //Multiplicado por 100?
            writeD(0); //Multiplicado por 100?
            writeC(0);
            writeH(20); //length de algo.
            writeB(new byte[20] { 0x70, 0x0C, 0x94, 0x2D, 0x48, 0x08, 0xDD, 0x1E, 0xB0, 0xAB, 0x1A, 0x00, 0x99, 0x7B, 0x42, 0x00, 0x70, 0x0C, 0x94, 0x2D });        
            writeD(c.IsGM()|| c.HaveAcessLevel());
            writeD(_xmas);
            writeC(1); //Repair?

            WriteVisitEvent(evVisit);

            if (cfg.ClientVersion == "1.15.39" || cfg.ClientVersion == "1.15.41" || cfg.ClientVersion == "1.15.42")
            {
                writeB(new byte[9]);
            }

            writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));//DataNow By Server
            //writeB(new byte[256]);
            writeS("10.120.1.44", 256); //?
            writeD(8085); //0
            writeC(cfg.GiftSystem); //Ativar presentes (0 = Não | 1 = Sim)

            writeH(1); //Lê algo se for positivo, Quantidade sequêncial (1byte)
            writeC(0);
            writeC(1);//1

            writeC(7);//7
                   
            writeC(4);//4 víp 1//Length dos write abaixo
            writeC(1);// vip2 Relevancia da itens tag VIP - 0 - Por Goods | 1 - Topo da lista | 2 - Fim da Lista | 3 - VIP Topo, New Good
            writeC(1);//1 new
            writeC(2);//2 hot
            writeC(5);//5 sale
            writeC(3);//3 event
            writeC(6);//6*/
        }
        private void WriteVisitEvent(EventVisitModel ev)
        {
            PlayerEvent pev = c._event;
            if (ev != null && (pev.LastVisitSequence1 < ev.checks && pev.NextVisitDate <= int.Parse(DateTime.Now.ToString("yyMMdd")) || pev.LastVisitSequence2 < ev.checks && pev.LastVisitSequence2 != pev.LastVisitSequence1))
            {
                writeD(ev.id);
                writeC((byte)pev.LastVisitSequence1);
                writeC((byte)pev.LastVisitSequence2);
                writeH(0);
                writeD(ev.startDate); //12
                writeS(ev.title, 60);
                writeC(2);
                writeC((byte)ev.checks);
                writeH(0);
                writeD(ev.id);
                writeD(ev.startDate);
                writeD(ev.endDate);
                for (int i = 0; i < 7; i++)
                {
                    VisitBox box = ev.box[i];
                    writeD(box.RewardCount);
                    writeD(box.reward1.good_id);
                    writeD(box.reward2.good_id);
                }
            }
            else
                writeB(new byte[172]);
        }
    }
}
/*
 * Arquivo: Room.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 08/01/2018
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.managers;
using Core.managers.events;
using Core.managers.server;
using Core.models.account.clan;
using Core.models.account.players;
using Core.models.account.rank;
using Core.models.enums;
using Core.models.enums.errors;
using Core.models.enums.flags;
using Core.models.enums.match;
using Core.models.room;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.sync;
using Game.data.sync.server_side;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game.data.model
{
    public class Room
    {
        public SLOT[] _slots = new SLOT[16];
        public int _channelType, rodada = 1, TRex = -1, blue_rounds, blue_dino, red_rounds, red_dino, Bar1, Bar2, _ping = 5, _redKills, _redDeaths, _blueKills, _blueDeaths, spawnsCount, mapId, autobalans, killtime, _roomId, _channelId, _leader;
        public byte weaponsFlag, random_map, special, limit, seeConf, aiCount = 1, IngameAiLevel, aiLevel, stage4v4, room_type;
        public readonly int[] TIMES = new int[9] { 3, 5, 7, 5, 10, 15, 20, 25, 30 }, KILLS = new int[6] { 60, 80, 100, 120, 140, 160 }, ROUNDS = new int[6] { 1, 2, 3, 5, 7, 9 },
            RED_TEAM = new int[8] { 0, 2, 4, 6, 8, 10, 12, 14 }, BLUE_TEAM = new int[8] { 1, 3, 5, 7, 9, 11, 13, 15 };
        public byte[] HitParts = new byte[35],
            DefaultParts = new byte[35] { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                30, 31, 32, 33, 34 };
        public uint _timeRoom, StartDate, UniqueRoomId;
        public long StartTick;
        public string name, password, _mapName;
        public VoteKick votekick;
        public RoomState _state;
        public bool C4_actived, swapRound, changingSlots, blockedClan;
        public BattleServer UDPServer;
        public DateTime BattleStart, LastPingSync = DateTime.Now;
        public TimerState bomba = new TimerState(), countdown = new TimerState(), round = new TimerState(), vote = new TimerState();
        public SafeList<long> kickedPlayers = new SafeList<long>(),
            requestHost = new SafeList<long>();
        public Room(int roomId, Channel ch)
        {
            _roomId = roomId;
            for (int i = 0; i < _slots.Length; ++i)
                _slots[i] = new SLOT(i);
            _channelId = ch._id;
            _channelType = ch._type;
            SetUniqueId();
        }
        /// <summary>
        /// Retorna TRUE se o modo atual tiver contagem regressiva.
        /// </summary>
        /// <returns></returns>
        public bool thisModeHaveCD()
        {
            RoomType stageType = (RoomType)room_type;
            return (stageType == RoomType.Explosion ||
                    stageType == RoomType.Annihilation ||
                    stageType == RoomType.Boss ||
                    stageType == RoomType.Cross_Counter ||
                    stageType == RoomType.Escort);
        }
        /// <summary>
        /// Retorna TRUE se o modo atual utilizar o contador de rodadas.
        /// </summary>
        /// <returns></returns>
        public bool thisModeHaveRounds()
        {
            RoomType stageType = (RoomType)room_type;
            return (stageType == RoomType.Explosion ||
                    stageType == RoomType.Destroy ||
                    stageType == RoomType.Annihilation ||
                    stageType == RoomType.Defense);
        }
        public void LoadHitParts()
        {
            Random rnd = new Random();
            int next = rnd.Next(34);
            byte[] MyRandomArray = DefaultParts.OrderBy(x => x <= next).ToArray();

            //byte first = MyRandomArray[14];
            //byte second = MyRandomArray[29];
            //MyRandomArray[29] = first;
            //MyRandomArray[14] = second;

            Logger.warning("By: " + next + "/ Hits: " + BitConverter.ToString(MyRandomArray));
            HitParts = MyRandomArray;

            byte[] newarray = new byte[35];
            //byte[] newarray2 = new byte[43];
            for (int i = 0; i < 35; i++)
            {
                byte valor = HitParts[i];
                newarray[((i + 8) % 35)] = valor;
            }
            Logger.warning("P: " + BitConverter.ToString(newarray));
        }
        private void SetUniqueId()
        {
            UniqueRoomId = (uint)((ConfigGS.serverId & 0xff) << 20 | (_channelId & 0xff) << 12 | _roomId & 0xfff);
        }
        public void SetBotLevel()
        {
            if (!isBotMode())
                return;
            IngameAiLevel = aiLevel;
            for (int i = 0; i < 16; i++)
                _slots[i].aiLevel = IngameAiLevel;
        }
        public bool isBotMode()
        {
            return special == 6 || special == 8 || special == 9;
        }
        private void SetSpecialStage()
        {
            if ((RoomType)room_type == RoomType.Defense)
            {
                if (mapId == 39)
                {
                    Bar1 = 6000;
                    Bar2 = 9000;
                }
            }
            else if ((RoomType)room_type == RoomType.Destroy)
            {
                if (mapId == 38)
                {
                    Bar1 = 12000;
                    Bar2 = 12000;
                }
                else if (mapId == 35)
                {
                    Bar1 = 6000;
                    Bar2 = 6000;
                }
            }
        }
        /// <summary>
        /// Retorna os segundos decorridos desde o inicio da partida.
        /// </summary>
        /// <returns>Tempo em segundos</returns>
        public int getInBattleTime()
        {
            int seconds = 0;
            if (BattleStart != new DateTime() && (_state == RoomState.Battle || _state == RoomState.PreBattle))
            {
                DateTime now = DateTime.Now;
                seconds = (int)(now - BattleStart).TotalSeconds;
                if (seconds < 0) seconds = 0;
            }
            return seconds;
        }
        /// <summary>
        /// Retorna o tempo restante que falta para a partida terminar.
        /// </summary>
        /// <returns>Tempo em segundos</returns>
        public int getInBattleTimeLeft()
        {
            int remaining = getInBattleTime();
            return ((getTimeByMask() * 60) - remaining);
        }
        public Channel getChannel()
        {
            return ChannelsXML.getChannel(_channelId);
        }
        public bool getChannel(out Channel ch)
        {
            ch = ChannelsXML.getChannel(_channelId);
            return ch != null;
        }
        public bool getSlot(int slotIdx, out SLOT slot)
        {
            slot = null;
            lock (_slots)
            {
                if (slotIdx >= 0 && slotIdx <= 15)
                    slot = _slots[slotIdx];
                return slot != null;
            }
        }
        public SLOT getSlot(int slotIdx)
        {
            lock (_slots)
            {
                if (slotIdx >= 0 && slotIdx <= 15)
                    return _slots[slotIdx];
                return null;
            }
        }
        /// <summary>
        /// Inicia o temporizador de DC do jogador.
        /// <para>Type = 0: (90 segundos)</para>
        /// <para>Type = 1: (30 segundos)</para>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="player"></param>
        /// <param name="slot"></param>
        public void StartCounter(int type, Account player, SLOT slot)
        {
            int dueTime = 0;
            EventErrorEnum error = 0;
            if (type == 0)
            {
                error = EventErrorEnum.Battle_First_MainLoad;
                dueTime = 90000;
            }
            else if (type == 1)
            {
                error = EventErrorEnum.Battle_First_Hole;
                dueTime = 30000;
            }
            else
                return;
            slot.timing.StartJob(dueTime, (callbackState) =>
            {
                BaseCounter(error, player, slot);
                lock (callbackState)
                {
                    if (slot != null)
                        slot.StopTiming();
                }
            });
        }
        private void BaseCounter(EventErrorEnum error, Account player, SLOT slot)
        {
            player.SendPacket(new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(error));
            player.SendPacket(new BATTLE_LEAVEP2PSERVER_PAK(player, 0));
            slot.state = SLOT_STATE.NORMAL;
            AllUtils.BattleEndPlayersCount(this, isBotMode());
            updateSlotsInfo();
        }
        public void StartBomb()
        {
            try
            {
                bomba.StartJob(42000, (callbackState) =>
                {
                    if (this != null && C4_actived)
                    {
                        red_rounds++;
                        C4_actived = false;
                        AllUtils.BattleEndRound(this, 0, RoundEndType.BombFire);
                    }
                    lock (callbackState)
                    { bomba.Timer = null; }
                });
            }
            catch (Exception ex) 
            { 
                Logger.warning("StartBomb: " + ex.ToString()); 
            }
        }
        public void StartVote()
        {
            try
            {
                if (votekick == null)
                    return;

                vote.StartJob(20000, (callbackState) =>
                {
                    AllUtils.votekickResult(this);
                    lock (callbackState)
                    { vote.Timer = null; }
                });
            }
            catch (Exception ex)
            {
                Logger.warning("[Room.StartVote] " + ex.ToString());
                if (vote.Timer != null)
                    vote.Timer = null;
                votekick = null;
            }
        }
        public void RoundRestart()
        {
            try
            {
                StopBomb();
                for (int i = 0; i < 16; i++)
                {
                    SLOT s = _slots[i];
                    if (s._playerId > 0 && (int)s.state == 13)
                    {
                        if (!s._deathState.HasFlag(DeadEnum.useChat))
                            s._deathState |= DeadEnum.useChat;
                        //if (s.morto && !s.specGM) s.morto = false;
                        if (s.espectador) s.espectador = false;
                        if (s.killsOnLife >= 3 && room_type == 4) s.objetivos++;
                        s.killsOnLife = 0;
                        s.lastKillState = 0;
                        s.repeatLastState = false;
                        s.damageBar1 = 0;
                        s.damageBar2 = 0;
                    }
                }
                round.StartJob(8000, (callbackState) =>
                {
                    for (int i = 0; i < 16; i++)
                    {
                        SLOT s = _slots[i];
                        if (s._playerId > 0)
                        {
                            if (!s._deathState.HasFlag(DeadEnum.useChat))
                                s._deathState |= DeadEnum.useChat;
                            if (s.espectador) s.espectador = false;
                        }
                    }
                    StopBomb();
                    DateTime now = DateTime.Now;
                    if (_state == RoomState.Battle)
                        BattleStart = room_type == 7 || room_type == 12 ? now.AddSeconds(5) : now;
                    using (BATTLE_ROUND_RESTART_PAK packet = new BATTLE_ROUND_RESTART_PAK(this))
                    using (BATTLE_TIMERSYNC_PAK packet2 = new BATTLE_TIMERSYNC_PAK(this))
                        SendPacketToPlayers(packet, packet2, SLOT_STATE.BATTLE, 0);
                    StopBomb();
                    swapRound = false;
                    AllUtils.LogRoomRoundRestart(this);
                    lock (callbackState)
                    { round.Timer = null; }
                });
            }
            catch (Exception ex) { Logger.warning("[Room.RoundRestart] " + ex.ToString()); }
        }
        public void StopBomb()
        {
            if (!C4_actived)
                return;
            C4_actived = false;
            if (bomba != null)
                bomba.Timer = null;
        }
        public void StartBattle(bool updateInfo)
        {
            Monitor.Enter(_slots);
            _state = RoomState.Loading;
            requestHost.Clear();
            UDPServer = BattleServerXML.GetRandomServer();
            StartTick = DateTime.Now.Ticks;
            StartDate = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
            SetBotLevel();
            AllUtils.CheckClanMatchRestrict(this);
            using (BATTLE_READYBATTLE_PAK packet = new BATTLE_READYBATTLE_PAK(this))
            {
                byte[] data = packet.GetCompleteBytes("Room.StartBattle");
                foreach (Account pR in getAllPlayers(SLOT_STATE.READY, 0))
                {
                    SLOT slot = getSlot(pR._slotId);
                    if (slot != null)
                    {
                        slot.withHost = true;
                        slot.state = SLOT_STATE.LOAD;
                        slot.SetMissionsClone(pR._mission);
                        pR.SendCompletePacket(data);
                    }
                }
            }
            if (updateInfo)
                updateSlotsInfo();
            Monitor.Exit(_slots);
        }
        public void StartCountDown()
        {
            using (BATTLE_COUNTDOWN_PAK packet = new BATTLE_COUNTDOWN_PAK(CountDownEnum.Start))
                SendPacketToPlayers(packet);
            countdown.StartJob(5250, (callbackState) =>
            {
                try
                {
                    if (_slots[_leader].state == SLOT_STATE.READY && _state == RoomState.CountDown)
                        StartBattle(true);
                    //else
                    //{
                    //    Logger.warning("[Room.StartCountDown] Sala: " + _roomId + "; Canal: " + _channelId + "; state: " + _state + "; leader: " + _slots[_leader].state);
                    //    Logger.warning("[Data: " + DateTime.Now.ToString("dd/MM/yy HH:mm:ss"));
                    //}
                }
                catch (Exception ex)
                {
                    Logger.warning("[Room.StartCountDown] " + ex.ToString());
                }
                lock (callbackState)
                { countdown.Timer = null; }
            });
        }
        /// <summary>
        /// Desativa a contagem regressiva e envia um pacote com o cancelamento da contagem.
        /// </summary>
        /// <param name="motive"></param>
        /// <param name="refreshRoom">Atualizar infos da sala?</param>
        public void StopCountDown(CountDownEnum motive, bool refreshRoom = true)
        {
            _state = RoomState.Ready;
            if (refreshRoom)
                updateRoomInfo();
            countdown.Timer = null;
            using (BATTLE_COUNTDOWN_PAK packet = new BATTLE_COUNTDOWN_PAK(motive))
                SendPacketToPlayers(packet);
        }
        /// <summary>
        /// Cancela a contagem regressiva se o slot do jogador for o dono da sala, ou se não houver jogadores no time oposto ao do dono da sala.
        /// </summary>
        /// <param name="slotId">Slot do jogador</param>
        public void StopCountDown(int slotId)
        {
            if (_state != RoomState.CountDown)
                return;
            if (slotId == _leader)
                StopCountDown(CountDownEnum.StopByHost);
            else if (getPlayingPlayers(_leader % 2 == 0 ? 1 : 0, SLOT_STATE.READY, 0) == 0)
            {
                changeSlotState(_leader, SLOT_STATE.NORMAL, false);
                StopCountDown(CountDownEnum.StopByPlayer);
            }
        }
        /// <summary>
        /// Calcula os resultados da partida. Gera automaticamente o time vencedor.
        /// </summary>
        public void CalculateResult()
        {
            lock (_slots)
                BaseResultGame(AllUtils.GetWinnerTeam(this), isBotMode());
        }
        /// <summary>
        /// Calcula os resultados da partida. Pode definir o time vencedor.
        /// </summary>
        /// <param name="resultType">Time vencedor</param>
        public void CalculateResult(TeamResultType resultType)
        {
            lock (_slots)
                BaseResultGame(resultType, isBotMode());
        }
        /// <summary>
        /// Calcula os resultados da partida. É possível definir o time vencedor e se a partida é contra I.A.
        /// </summary>
        /// <param name="resultType">Time vencedor</param>
        /// <param name="isBotMode">Partida contra I.A?</param>
        public void CalculateResult(TeamResultType resultType, bool isBotMode)
        {
            lock (_slots)
                BaseResultGame(resultType, isBotMode);
        }
        private void BaseResultGame(TeamResultType winnerTeam, bool isBotMode)
        {
            ServerConfig cfg = GameManager.Config;
            EventUpModel evUp = EventRankUpSyncer.getRunningEvent();
            EventMapModel evMap = EventMapSyncer.getRunningEvent();
            bool mapEvUse = EventMapSyncer.EventIsValid(evMap, mapId, room_type);
            PlayTimeModel evPt = EventPlayTimeSyncer.getRunningEvent();
            DateTime finishDate = DateTime.Now;
            if (cfg == null)
            {
                Logger.error("Server Config NULL. RoomResult canceled.");
                return;
            }
            for (int i = 0; i < 16; ++i)
            {
                SLOT slot = _slots[i];
                Account player;
                if (!slot.check && slot.state == SLOT_STATE.BATTLE && getPlayerBySlot(slot, out player))
                {
                    DBQuery query = new DBQuery();
                    slot.check = true;

                    double inBattleTime = slot.inBattleTime(finishDate);
                    int gp = player._gp, xp = player._exp, my = player._money;
                    if (!isBotMode)
                    {
                        if (cfg.missions)
                        {
                            AllUtils.endMatchMission(this, player, slot, winnerTeam);
                            if (slot.MissionsCompleted)
                            {
                                player._mission = slot.Missions;
                                MissionManager.getInstance().updateCurrentMissionList(player.player_id, player._mission);
                            }
                            AllUtils.GenerateMissionAwards(player, query);
                        }
                        int timePlayed = slot.allKills == 0 && slot.allDeaths == 0 ? (int)(inBattleTime / 3) : (int)inBattleTime;

                        if (room_type == 2 || room_type == 4)
                        {
                            slot.exp = (int)(slot.Score + (timePlayed / 2.5) + (slot.allDeaths * 2.2) + (slot.objetivos * 20));
                            slot.gp = (int)(slot.Score + (timePlayed / 3.0) + (slot.allDeaths * 2.2) + (slot.objetivos * 20));
                            slot.money = (int)((slot.Score / 2) + (timePlayed / 6.5) + (slot.allDeaths * 1.5) + (slot.objetivos * 10));
                        }
                        else
                        {
                            slot.exp = (int)(slot.Score + (timePlayed / 2.5) + (slot.allDeaths * 1.8) + (slot.objetivos * 20));
                            slot.gp = (int)(slot.Score + (timePlayed / 3.0) + (slot.allDeaths * 1.8) + (slot.objetivos * 20));
                            slot.money = (int)((slot.Score/1.5) + (timePlayed / 4.5) + (slot.allDeaths * 1.1) + (slot.objetivos * 20));
                        }
                        bool WonTheMatch = (slot._team == (int)winnerTeam);
                        if ((RoomType)room_type != RoomType.Chaos && (RoomType)room_type != RoomType.Head_Hunter)
                        {
                            player._statistic.headshots_count += slot.headshots;
                            player._statistic.kills_count += slot.allKills;
                            player._statistic.totalkills_count += slot.allKills;
                            player._statistic.deaths_count += slot.allDeaths;
                            AddKDInfosToQuery(slot, player._statistic, query);
                            AllUtils.updateMatchCount(WonTheMatch, player, (int)winnerTeam, query);
                        }
                        if (WonTheMatch)
                        {
                            slot.gp += AllUtils.percentage(slot.gp, 15);
                            slot.exp += AllUtils.percentage(slot.exp, 20);
                        }
                        if (slot.earnedXP > 0)
                            slot.exp += slot.earnedXP * 5;
                    }
                    else
                    {
                        int level = IngameAiLevel * (150 + slot.allDeaths);
                        if (level == 0)
                            level++;
                        int reward = (slot.Score / level);
                        slot.gp += reward;
                        slot.exp += reward;
                    }
                    slot.exp = slot.exp > ConfigGS.maxBattleXP ? ConfigGS.maxBattleXP : slot.exp;
                    slot.gp = slot.gp > ConfigGS.maxBattleGP ? ConfigGS.maxBattleGP : slot.gp;
                    slot.money = slot.money > ConfigGS.maxBattleMY ? ConfigGS.maxBattleMY : slot.money;
                    if (slot.exp < 0 || slot.gp < 0 || slot.money < 0)
                    {
                        Logger.warning("[1]SLOT XP: " + slot.exp + "; GOLD: " + slot.gp + "; MONEY: " + slot.money);
                        Logger.warning("InBattleTime: " + inBattleTime + " [Finish: " + finishDate.ToString("yyMMddHHmmss") + "; Start: " + slot.startTime.ToString("yyMMddHHmmss") + "]");
                        Logger.warning("allDeaths: " + slot.allDeaths + "; Objs: " + slot.objetivos + "; IsBOT: " + isBotMode);
                        slot.exp = 2;
                        slot.gp = 2;
                        slot.money = 2;
                    }
                    int pctgXP = 0, pctgGP = 0;
                    if (evUp != null || mapEvUse)
                    {
                        if (evUp != null)
                        {
                            pctgXP += evUp._percentXp;
                            pctgGP += evUp._percentGp;
                        }
                        if (mapEvUse)
                        {
                            pctgXP += evMap._percentXp;
                            pctgGP += evMap._percentGp;
                        }
                        if (!slot.bonusFlags.HasFlag(ResultIcon.Event))
                            slot.bonusFlags |= ResultIcon.Event;
                    }
                    PlayerBonus c = player._bonus;
                    if (c != null && c.bonuses > 0)
                    {
                        if ((c.bonuses & 8) == 8)
                            pctgXP += 100;
                        if ((c.bonuses & 128) == 128)
                            pctgGP += 100;
                        if ((c.bonuses & 4) == 4)
                            pctgXP += 50;
                        if ((c.bonuses & 64) == 64)
                            pctgGP += 50;
                        if ((c.bonuses & 2) == 2)
                            pctgXP += 30;
                        if ((c.bonuses & 32) == 32)
                            pctgGP += 30;
                        if ((c.bonuses & 1) == 1)
                            pctgXP += 10;
                        if ((c.bonuses & 16) == 16)
                            pctgGP += 10;
                        if (!slot.bonusFlags.HasFlag(ResultIcon.Item))
                            slot.bonusFlags |= ResultIcon.Item;
                    }
                    if (player.pc_cafe == 2 || player.pc_cafe == 1)
                    {
                        pctgXP += player.pc_cafe == 2 ? 120 : 60;//60 : 30;
                        pctgGP += player.pc_cafe == 2 ? 100 : 40;//50 : 20;
                        if (player.pc_cafe == 1 && !slot.bonusFlags.HasFlag(ResultIcon.Pc))
                            slot.bonusFlags |= ResultIcon.Pc;
                        else if (player.pc_cafe == 2 && !slot.bonusFlags.HasFlag(ResultIcon.PcPlus))
                            slot.bonusFlags |= ResultIcon.PcPlus;
                    }
                    slot.BonusXP = AllUtils.percentage(slot.exp, pctgXP);
                    slot.BonusGP = AllUtils.percentage(slot.gp, pctgGP);
                    player._gp += slot.gp + slot.BonusGP;
                    player._exp += slot.exp + slot.BonusXP;

                    if (ConfigGS.winCashPerBattle)
                        player._money += slot.money;
                    RankModel rank = RankXML.getRank(player._rank);
                    if (rank != null && player._exp >= (rank._onNextLevel + rank._onAllExp) && player._rank <= 50)
                    {
                        List<ItemsModel> items = RankXML.getAwards(player._rank);
                        if (items.Count > 0)
                            player.SendPacket(new INVENTORY_ITEM_CREATE_PAK(1, player, items));
                        player._gp += rank._onGPUp;
                        player.LastRankUpDate = uint.Parse(finishDate.ToString("yyMMddHHmm"));
                        player.SendPacket(new BASE_RANK_UP_PAK(++player._rank, rank._onNextLevel));
                        query.AddQuery("last_rankup_date", (long)player.LastRankUpDate);
                        query.AddQuery("rank", player._rank);
                    }
                    if (evPt != null)
                        AllUtils.playTimeEvent((long)inBattleTime, player, evPt, isBotMode);
                    AllUtils.DiscountPlayerItems(slot, player);
                    if (gp != player._gp)
                        query.AddQuery("gp", player._gp);
                    if (xp != player._exp)
                        query.AddQuery("exp", player._exp);
                    if (my != player._money)
                        query.AddQuery("money", player._money);
                    ComDiv.updateDB("accounts", "player_id", player.player_id, query.GetTables(), query.GetValues());
                    if (ConfigGS.winCashPerBattle && ConfigGS.showCashReceiveWarn)
                        player.SendPacket(new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("CashReceived", slot.money)));
                }
            }
            updateSlotsInfo();
            CalculateClanMatchResult((int)winnerTeam);
        }
        private void AddKDInfosToQuery(SLOT slot, PlayerStats stats, DBQuery query)
        {
            if (slot.allKills > 0)
            {
                query.AddQuery("kills_count", stats.kills_count);
                query.AddQuery("totalkills_count", stats.totalkills_count);
            }
            if (slot.allDeaths > 0)
                query.AddQuery("deaths_count", stats.deaths_count);
            if (slot.headshots > 0)
                query.AddQuery("headshots_count", stats.headshots_count);
        }
        private void CalculateClanMatchResult(int winnerTeam)
        {
            if (_channelType != 4 || blockedClan)
                return;
            SortedList<int, Clan> list = new SortedList<int, Clan>();
            for (int i = 0; i < 16; i++)
            {
                SLOT slot = _slots[i];
                Account p;
                if (slot.state == SLOT_STATE.BATTLE && getPlayerBySlot(slot, out p))
                {
                    Clan clan = ClanManager.getClan(p.clanId);
                    if (clan._id == 0)
                        continue;
                    bool WonTheMatch = (slot._team == winnerTeam);
                    clan._exp += slot.exp;
                    clan.BestPlayers.SetBestExp(slot);
                    clan.BestPlayers.SetBestKills(slot);
                    clan.BestPlayers.SetBestHeadshot(slot);
                    clan.BestPlayers.SetBestWins(p._statistic, slot, WonTheMatch);
                    clan.BestPlayers.SetBestParticipation(p._statistic, slot);
                    if (!list.ContainsKey(p.clanId))
                    {
                        list.Add(p.clanId, clan);
                        if (winnerTeam == 2)
                            goto UpdateClanFights;
                        CalculateSpecialCM(clan, winnerTeam, slot._team);
                        if (WonTheMatch)
                            clan.vitorias++;
                        else
                            clan.derrotas++;
                    UpdateClanFights:
                        PlayerManager.updateClanBattles(clan._id, ++clan.partidas, clan.vitorias, clan.derrotas);
                    }
                }
            }
            foreach (Clan clan in list.Values)
            {
                PlayerManager.updateClanExp(clan._id, clan._exp);
                if (special == 5)
                    PlayerManager.updateClanPoints(clan._id, clan._pontos);
                PlayerManager.updateBestPlayers(clan);
                RankModel rankModel = ClanRankXML.getRank(clan._rank);
                if (rankModel != null && clan._exp >= rankModel._onNextLevel + rankModel._onAllExp)
                    PlayerManager.updateClanRank(clan._id, ++clan._rank);
            }
        }
        private void CalculateSpecialCM(Clan clan, int winnerTeam, int teamIdx)
        {
            if (special != 5 || winnerTeam == 2)
                return;
            if (winnerTeam == teamIdx)
            {
                float morePoints = 0;
                if (room_type == 1)
                    morePoints = (teamIdx == 0 ? _redKills : _blueKills) / 20;
                else
                    morePoints = (teamIdx == 0 ? red_rounds : blue_rounds);
                float POINTS = (25 + morePoints);
                Logger.warning("Clan: " + clan._id + "; Earned Points: " + POINTS);
                clan._pontos += POINTS;
                Logger.warning("Clan: " + clan._id + "; Final Points: " + clan._pontos);
            }
            else
            {
                if (clan._pontos == 0)
                {
                    Logger.warning("Clã não perdeu Pontos devido a baixa pontuação.");
                    return;
                }
                float morePoints = 0;
                if (room_type == 1)
                    morePoints = (teamIdx == 0 ? _redKills : _blueKills) / 20;
                else
                    morePoints = (teamIdx == 0 ? red_rounds : blue_rounds);
                float POINTS = (40 - morePoints);
                Logger.warning("Clan: " + clan._id + "; Losed Points: " + POINTS);
                clan._pontos -= POINTS;
                Logger.warning("Clan: " + clan._id + "; Final Points: " + clan._pontos);
            }
        }
        /// <summary>
        /// Se estiver em contagem regressiva, preparação, ou em batalha, retorna TRUE, caso contrário FALSE.
        /// <para>(state > RoomState.Empty)</para>
        /// </summary>
        /// <returns></returns>
        public bool isStartingMatch()
        {
            return _state > 0;
        }
        /// <summary>
        /// Se estiver em preparação, ou em batalha, retorna TRUE, caso contrário FALSE.
        /// <para>Não funciona em contagem regressiva.</para>
        /// <para>(state >= RoomState.Loading)</para>
        /// </summary>
        /// <returns></returns>
        public bool isPreparing()
        {
            return (int)_state >= 2;
        }
        /// <summary>
        /// Atualiza as informações da sala, para todos os jogadores.
        /// </summary>
        public void updateRoomInfo()
        {
            using (BATTLE_ROOM_INFO_PAK packet = new BATTLE_ROOM_INFO_PAK(this))
                SendPacketToPlayers(packet);
        }
        public void initSlotCount(int count)
        {
            if (stage4v4 == 1)
                count = 8;
            if (count <= 0)
                count = 1;
            for (int index = 0; index < _slots.Length; ++index)
                if (index >= count)
                    _slots[index].state = SLOT_STATE.CLOSE;
        }

        public int getSlotCount()
        {
            lock (_slots)
            {
                int count = 0;
                for (int index = 0; index < _slots.Length; ++index)
                    if (_slots[index].state != SLOT_STATE.CLOSE)
                        ++count;
                return count;
            }
        }

        /// <summary>
        /// Procura um novo slot vazio para fazer a troca do slot da conta.
        /// </summary>
        /// <param name="slots">Lista de mudanças</param>
        /// <param name="p">Conta</param>
        /// <param name="old">Slot antigo</param>
        /// <param name="teamIdx">Time</param>
        public void SwitchNewSlot(List<SLOT_CHANGE> slots, Account p, SLOT old, int teamIdx)
        {
            foreach (int index in GetTeamArray(teamIdx))
            {
                SLOT slot = _slots[index];
                if (slot._playerId == 0 && (int)slot.state == 0)
                {
                    slot.state = SLOT_STATE.NORMAL;
                    slot._playerId = p.player_id;
                    slot._equip = p._equip;

                    old.state = SLOT_STATE.EMPTY;
                    old._playerId = 0;
                    old._equip = null;

                    if (p._slotId == _leader)
                        _leader = index;
                    Logger.LogProblems("[Room.SwitchNewSlot] Jogador '" + p.player_id + "' '" + p.player_name + "'; OldSlot: " + p._slotId + "; NewSlot: " + index, "errorC");
                    p._slotId = index;
                    slots.Add(new SLOT_CHANGE { oldSlot = old, newSlot = slot });
                    break;
                }
            }
        }
        /// <summary>
        /// Troca todas as informações de 2 slots.
        /// </summary>
        /// <param name="slots">Lista de mudanças</param>
        /// <param name="newSlotId">Novo slot</param>
        /// <param name="oldSlotId">Antigo slot</param>
        /// <param name="changeReady">Caso estado estiver como Ready, mudar para Normal?</param>
        /// <returns></returns>
        public void SwitchSlots(List<SLOT_CHANGE> slots, int newSlotId, int oldSlotId, bool changeReady)
        {
            SLOT newSLOT = _slots[newSlotId];
            SLOT oldSLOT = _slots[oldSlotId];

            if (changeReady)
            {
                if (newSLOT.state == SLOT_STATE.READY)
                    newSLOT.state = SLOT_STATE.NORMAL;
                if (oldSLOT.state == SLOT_STATE.READY)
                    oldSLOT.state = SLOT_STATE.NORMAL;
            }

            newSLOT.SetSlotId(oldSlotId);
            oldSLOT.SetSlotId(newSlotId);

            _slots[newSlotId] = oldSLOT;
            _slots[oldSlotId] = newSLOT;
            slots.Add(new SLOT_CHANGE { oldSlot = newSLOT, newSlot = oldSLOT });
        }
        /// <summary>
        /// Troca o estado do slot do jogador.
        /// <para>Se for Empty/Close, ele reseta as informações do slot.</para>
        /// <para>Nenhum código é executado se o estado já for o informado.</para>
        /// </summary>
        /// <param name="slot">Slot</param>
        /// <param name="state">Novo estado</param>
        /// <param name="sendInfo">Atualizar informações para todos os outros jogadores?</param>
        public void changeSlotState(int slotId, SLOT_STATE state, bool sendInfo)
        {
            SLOT slot = getSlot(slotId);
            changeSlotState(slot, state, sendInfo);
        }
        /// <summary>
        /// Troca o estado do slot do jogador.
        /// <para>Se for Empty/Close, ele reseta as informações do slot.</para>
        /// <para>Nenhum código é executado se o estado já for o informado.</para>
        /// </summary>
        /// <param name="slot">Slot</param>
        /// <param name="state">Novo estado</param>
        /// <param name="sendInfo">Atualizar informações para todos os outros jogadores?</param>
        public void changeSlotState(SLOT slot, SLOT_STATE state, bool sendInfo)
        {
            if (slot == null || slot.state == state)
                return;
            slot.state = state;
            if ((int)state == 0 || (int)state == 1)
            {
                AllUtils.ResetSlotInfo(this, slot, false);
                slot._playerId = 0;
            }
            if (sendInfo)
                updateSlotsInfo();
        }
        public Account getPlayerBySlot(SLOT slot)
        {
            try
            {
                long id = slot._playerId;
                return id > 0 ? AccountManager.getAccount(id, true) : null;
            }
            catch
            {
                return null;
            }
        }
        public Account getPlayerBySlot(int slotId)
        {
            try
            {
                long id = _slots[slotId]._playerId;
                return id > 0 ? AccountManager.getAccount(id, true) : null;
            }
            catch
            {
                return null;
            }
        }
        public bool getPlayerBySlot(int slotId, out Account player)
        {
            try
            {
                long id = _slots[slotId]._playerId;
                player = id > 0 ? AccountManager.getAccount(id, true) : null;
                return player != null;
            }
            catch
            {
                player = null;
                return false;
            }
        }
        public bool getPlayerBySlot(SLOT slot, out Account player)
        {
            try
            {
                long id = slot._playerId;
                player = id > 0 ? AccountManager.getAccount(id, true) : null;
                return player != null;
            }
            catch
            {
                player = null;
                return false;
            }
        }

        public int getTimeByMask()
        {
            return TIMES[killtime >> 4];
        }
        public int getRoundsByMask()
        {
            return ROUNDS[killtime & 15];
        }
        public int getKillsByMask()
        {
            return KILLS[killtime & 15];
        }
        /// <summary>
        /// Atualiza as informações dos slots da sala, para todos os jogadores.
        /// </summary>
        public void updateSlotsInfo()
        {
            using (ROOM_GET_SLOTINFO_PAK packet = new ROOM_GET_SLOTINFO_PAK(this))
                SendPacketToPlayers(packet);
        }
        /// <summary>
        /// Pega as informações da conta do dono da sala. Caso a quantidade de jogadores for 0, é retornado FALSO. Se não houver líder, será feita uma tentativa de troca de dono.
        /// </summary>
        /// <param name="p">Conta do dono da sala</param>
        /// <returns></returns>
        public bool getLeader(out Account p)
        {
            p = null;
            if (getAllPlayers().Count <= 0)
                return false;
            if (_leader == -1)
                setNewLeader(-1, 0, -1, false);
            if (_leader >= 0)
                p = AccountManager.getAccount(_slots[_leader]._playerId, true);
            return p != null;
        }
        /// <summary>
        /// Pega as informações da conta do dono da sala. Caso a quantidade de jogadores for 0, é retornado FALSO. Se não houver líder, será feita uma tentativa de troca de dono.
        /// </summary>
        /// <returns></returns>
        public Account getLeader()
        {
            if (getAllPlayers().Count <= 0)
                return null;
            if (_leader == -1)
                setNewLeader(-1, 0, -1, false);
            return _leader == -1 ? null : AccountManager.getAccount(_slots[_leader]._playerId, true);
        }
        /// <summary>
        /// Indica um novo líder da sala. Seguindo alguns paramêtros. Se o novo dono estiver com PRONTO, ele será retirado automaticamente.
        /// </summary>
        /// <param name="leader">Slot do líder. (-1 = Seleção aleatória)</param>
        /// <param name="state">Indicar um novo através do estado do slot do jogador. (O estado tem que ser maior do que o indicado)</param>
        /// <param name="oldLeader">Ignorar um certo slot. (-1 = Nenhum)</param>
        /// <param name="updateInfo">Utilizar o recurso de atualizar slot das salas?</param>
        public void setNewLeader(int leader, int state, int oldLeader, bool updateInfo)
        {
            Monitor.Enter(_slots);
            if (leader == -1)
            {
                for (int i = 0; i < 16; ++i)
                {
                    SLOT slot = _slots[i];
                    if (i != oldLeader && slot._playerId > 0 && (int)slot.state > state)
                    {
                        _leader = i;
                        break;
                    }
                }
            }
            else
                _leader = leader;
            if (_leader != -1)
            {
                SLOT slot = _slots[_leader];
                if ((int)slot.state == 8)
                    slot.state = SLOT_STATE.NORMAL;
                if (updateInfo)
                    updateSlotsInfo();
            }
            Monitor.Exit(_slots);
        }
        public void SendPacketToPlayers(SendPacket packet)
        {
            List<Account> players = getAllPlayers();
            if (players.Count == 0)
                return;

            byte[] data = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket)");
            for (int i = 0; i < players.Count; i++)
                players[i].SendCompletePacket(data);
        }
        public void SendPacketToPlayers(SendPacket packet, long player_id)
        {
            List<Account> players = getAllPlayers(player_id);
            if (players.Count == 0)
                return;

            byte[] data = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,long)");
            for (int i = 0; i < players.Count; i++)
                players[i].SendCompletePacket(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        public void SendPacketToPlayers(SendPacket packet, SLOT_STATE state, int type)
        {
            List<Account> players = getAllPlayers(state, type);
            if (players.Count == 0)
                return;
            byte[] data = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int)");
            for (int i = 0; i < players.Count; i++)
                players[i].SendCompletePacket(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="packet2"></param>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        public void SendPacketToPlayers(SendPacket packet, SendPacket packet2, SLOT_STATE state, int type)
        {
            List<Account> players = getAllPlayers(state, type);
            if (players.Count == 0)
                return;

            byte[] data = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-1");
            byte[] data2 = packet2.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-2");
            for (int i = 0; i < players.Count; i++)
            {
                Account pR = players[i];
                pR.SendCompletePacket(data);
                pR.SendCompletePacket(data2);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        /// <param name="exception"></param>
        public void SendPacketToPlayers(SendPacket packet, SLOT_STATE state, int type, int exception)
        {
            List<Account> players = getAllPlayers(state, type, exception);
            if (players.Count == 0)
                return;

            byte[] data = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int)");
            for (int i = 0; i < players.Count; i++)
                players[i].SendCompletePacket(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        /// <param name="exception"></param>
        /// <param name="exception2"></param>
        public void SendPacketToPlayers(SendPacket packet, SLOT_STATE state, int type, int exception, int exception2)
        {
            List<Account> players = getAllPlayers(state, type, exception, exception2);
            if (players.Count == 0)
                return;

            byte[] data = packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int,int)");
            for (int i = 0; i < players.Count; i++)
                players[i].SendCompletePacket(data);
        }
        public void RemovePlayer(Account player, bool WarnAllPlayers, int quitMotive = 0)
        {
            SLOT slot;
            if (player == null || !getSlot(player._slotId, out slot))
                return;
            BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
        }
        public void RemovePlayer(Account player, SLOT slot, bool WarnAllPlayers, int quitMotive = 0)
        {
            if (player == null || slot == null)
                return;
            BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
        }
        /// <summary>
        /// Remove um jogador da sala.
        /// </summary>
        /// <param name="player">Conta do jogador</param>
        /// <param name="slot">Slot do jogador</param>
        /// <param name="WarnAllPlayers">Enviar aviso de saída para o jogador também?</param>
        /// <param name="quitMotive">Motivo da saída. 0 = Normal; 1 = GM; 2 = Votação</param>
        private void BaseRemovePlayer(Account player, SLOT slot, bool WarnAllPlayers, int quitMotive)
        {
            Monitor.Enter(_slots);
            bool useRoomUpdate = false, hostChanged = false;
            if (player != null && slot != null)
            {
                if ((int)slot.state >= 9)
                {
                    if (_leader == slot._id)
                    {
                        int oldLeader = _leader,
                            bestState = 1; //Maior que Close
                        if (_state == RoomState.Battle)
                            bestState = 12; //Maior que BattleReady
                        else if ((int)_state >= 2)
                            bestState = 8; //Maior que Ready
                        if (getAllPlayers(slot._id).Count >= 1)
                            setNewLeader(-1, bestState, _leader, false);
                        if (getPlayingPlayers(2, SLOT_STATE.READY, 1) >= 2)
                        {
                            using (BATTLE_GIVEUPBATTLE_PAK packet = new BATTLE_GIVEUPBATTLE_PAK(this, oldLeader))
                                SendPacketToPlayers(packet, SLOT_STATE.RENDEZVOUS, 1, slot._id);
                        }
                        hostChanged = true;
                    }
                    using (BATTLE_LEAVEP2PSERVER_PAK packet = new BATTLE_LEAVEP2PSERVER_PAK(player, quitMotive))
                        SendPacketToPlayers(packet, SLOT_STATE.READY, 1, !WarnAllPlayers ? slot._id : -1);
                    BATTLE_LEAVE_SYNC.SendUDPPlayerLeave(this, slot._id);
                    slot.ResetSlot();
                    if (votekick != null)
                        votekick.TotalArray[slot._id] = false;
                }
                slot._playerId = 0;
                slot._equip = null;
                slot.state = SLOT_STATE.EMPTY;
                if ((int)_state == 1)
                {
                    if (slot._id == _leader)
                    {
                        _state = RoomState.Ready;
                        useRoomUpdate = true;
                        countdown.Timer = null;
                        using (BATTLE_COUNTDOWN_PAK packet = new BATTLE_COUNTDOWN_PAK(CountDownEnum.StopByHost))
                            SendPacketToPlayers(packet);
                    }
                    else if (getPlayingPlayers(slot._team, SLOT_STATE.READY, 0) == 0)
                    {
                        if (slot._id != _leader)
                            changeSlotState(_leader, SLOT_STATE.NORMAL, false);
                        StopCountDown(CountDownEnum.StopByPlayer, false);
                        useRoomUpdate = true;
                    }
                }
                else if (isPreparing())
                {
                    AllUtils.BattleEndPlayersCount(this, isBotMode());
                    if ((int)_state == 5)
                        AllUtils.BattleEndRoundPlayersCount(this);
                }
                CheckToEndWaitingBattle(hostChanged);
                requestHost.Remove(player.player_id);
                if (vote.Timer != null && votekick != null && votekick.victimIdx == player._slotId && quitMotive != 2)
                {
                    vote.Timer = null;
                    votekick = null;
                    using (VOTEKICK_CANCEL_VOTE_PAK packet = new VOTEKICK_CANCEL_VOTE_PAK())
                        SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                }
                Match match = player._match;
                if (match != null && player.matchSlot >= 0)
                {
                    match._slots[player.matchSlot].state = SlotMatchState.Normal;
                    using (CLAN_WAR_REGIST_MERCENARY_PAK packet = new CLAN_WAR_REGIST_MERCENARY_PAK(match))
                        match.SendPacketToPlayers(packet);
                }
                player._room = null;
                Logger.LogProblems("[Room.RemovePlayer] Jogador '" + player.player_id + "' '" + player.player_name + "'; OldSlot: " + player._slotId + "; NewSlot: -1", "errorC");
                player._slotId = -1;
                player._status.updateRoom(255);
                AllUtils.syncPlayerToClanMembers(player);
                AllUtils.syncPlayerToFriends(player, false);
                player.updateCacheInfo();
            }
            updateSlotsInfo();
            if (useRoomUpdate)
                updateRoomInfo();
            Monitor.Exit(_slots);
        }
        public int addPlayer(Account p)
        {
            lock (_slots)
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = _slots[i];
                    if (slot._playerId == 0 && (int)slot.state == 0)
                    {
                        slot._playerId = p.player_id;
                        slot.state = SLOT_STATE.NORMAL;
                        p._room = this;
                        Logger.LogProblems("[Room.addPlayer] Jogador '" + p.player_id + "' '" + p.player_name + "'; OldSlot: " + p._slotId + "; NewSlot: " + i, "errorC");
                        p._slotId = i;
                        slot._equip = p._equip;
                        p._status.updateRoom((byte)_roomId);
                        AllUtils.syncPlayerToClanMembers(p);
                        AllUtils.syncPlayerToFriends(p, false);
                        p.updateCacheInfo();
                        return i;
                    }
                }
            return -1;
        }
        public int addPlayer(Account p, int teamIdx)
        {
            int[] array = GetTeamArray(teamIdx);
            lock (_slots)
                for (int i = 0; i < array.Length; i++)
                {
                    int SlotIdx = array[i];
                    SLOT slot = _slots[SlotIdx];
                    if (slot._playerId == 0 && (int)slot.state == 0)
                    {
                        slot._playerId = p.player_id;
                        slot.state = SLOT_STATE.NORMAL;
                        p._room = this;
                        Logger.LogProblems("[Room.addPlayer2] Jogador '" + p.player_id + "' '" + p.player_name + "'; OldSlot: " + p._slotId + "; NewSlot: " + i, "errorC");
                        p._slotId = SlotIdx;
                        slot._equip = p._equip;
                        p._status.updateRoom((byte)_roomId);
                        AllUtils.syncPlayerToClanMembers(p);
                        AllUtils.syncPlayerToFriends(p, false);
                        p.updateCacheInfo();
                        return SlotIdx;
                    }
                }
            return -1;
        }
        public int[] GetTeamArray(int index)
        {
            return (index == 0 ? RED_TEAM : BLUE_TEAM);
        }
        /// <summary>
        /// Gera uma lista das contas dos jogadores.
        /// </summary>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        /// <returns></returns>
        public List<Account> getAllPlayers(SLOT_STATE state, int type)
        {
            List<Account> list = new List<Account>();
            lock (_slots)
                for (int i = 0; i < _slots.Length; ++i)
                {
                    SLOT slot = _slots[i];
                    long id = slot._playerId;
                    if (id > 0 && (type == 0 && slot.state == state || type == 1 && slot.state > state))
                    {
                        Account player = AccountManager.getAccount(id, true);
                        if (player != null && player._slotId != -1)
                            list.Add(player);
                    }
                }
            return list;
        }
        /// <summary>
        /// Gera uma lista das contas dos jogadores, com uma excessão de slot.
        /// </summary>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        /// <param name="exception">Slot do jogador</param>
        /// <returns></returns>
        public List<Account> getAllPlayers(SLOT_STATE state, int type, int exception)
        {
            List<Account> list = new List<Account>();
            lock (_slots)
                for (int i = 0; i < _slots.Length; ++i)
                {
                    SLOT slot = _slots[i];
                    long id = slot._playerId;
                    if (id > 0 && i != exception && (type == 0 && slot.state == state || type == 1 && slot.state > state))
                    {
                        Account player = AccountManager.getAccount(id, true);
                        if (player != null && player._slotId != -1)
                            list.Add(player);
                    }
                }
            return list;
        }
        /// <summary>
        /// Gera uma lista das contas dos jogadores, com duas excessões de slot.
        /// </summary>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        /// <param name="exception">Slot do jogador</param>
        /// <param name="exception2">Slot do jogador (2)</param>
        /// <returns></returns>
        public List<Account> getAllPlayers(SLOT_STATE state, int type, int exception, int exception2)
        {
            List<Account> list = new List<Account>();
            lock (_slots)
                for (int i = 0; i < _slots.Length; ++i)
                {
                    SLOT slot = _slots[i];
                    long id = slot._playerId;
                    if (id > 0 && i != exception && i != exception2 && (type == 0 && slot.state == state || type == 1 && slot.state > state))
                    {
                        Account player = AccountManager.getAccount(id, true);
                        if (player != null && player._slotId != -1)
                            list.Add(player);
                    }
                }
            return list;
        }
        /// <summary>
        /// Gera uma lista das contas dos jogadores, com uma excessão de slot.
        /// </summary>
        /// <param name="exception">Slot do jogador</param>
        /// <returns></returns>
        public List<Account> getAllPlayers(int exception)
        {
            List<Account> list = new List<Account>();
            lock (_slots)
                for (int i = 0; i < _slots.Length; ++i)
                {
                    long id = _slots[i]._playerId;
                    if (id > 0 && i != exception)
                    {
                        Account player = AccountManager.getAccount(id, true);
                        if (player != null && player._slotId != -1)
                            list.Add(player);
                    }
                }
            return list;
        }
        /// <summary>
        /// Gera uma lista das contas dos jogadores, com uma excessão de id.
        /// </summary>
        /// <param name="exception">Id do jogador</param>
        /// <returns></returns>
        public List<Account> getAllPlayers(long exception)
        {
            List<Account> list = new List<Account>();
            lock (_slots)
                for (int i = 0; i < _slots.Length; ++i)
                {
                    long id = _slots[i]._playerId;
                    if (id > 0 && id != exception)
                    {
                        Account player = AccountManager.getAccount(id, true);
                        if (player != null && player._slotId != -1)
                            list.Add(player);
                    }
                }
            return list;
        }
        /// <summary>
        /// Gera uma lista das contas dos jogadores.
        /// </summary>
        /// <returns></returns>
        public List<Account> getAllPlayers()
        {
            List<Account> list = new List<Account>();
            lock (_slots)
                for (int i = 0; i < _slots.Length; ++i)
                {
                    long id = _slots[i]._playerId;
                    if (id > 0)
                    {
                        Account player = AccountManager.getAccount(id, true);
                        if (player != null && player._slotId != -1)
                            list.Add(player);
                    }
                }
            return list;
        }
        /// <summary>
        /// Se inBattle for TRUE pega slots com "state" = BATTLE e "espectador" = FALSE.
        /// <para>Se inBattle for FALSE pega slots com "state" >= RENDEZVOUS.</para>
        /// </summary>
        /// <param name="team">Time</param>
        /// <param name="inBattle">Pegar jogadores em partida?</param>
        /// <returns></returns>
        public int getPlayingPlayers(int team, bool inBattle)
        {
            int players = 0;
            lock (_slots)
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = _slots[i];
                    if (slot._playerId > 0 && (slot._team == team || team == 2) && (inBattle && (int)slot.state == 13 && !slot.espectador || !inBattle && (int)slot.state >= 10))
                        players++;
                }
            return players;
        }
        /// <summary>
        /// Retorna a quantidade de jogadores, seguindo alguns parâmetros.
        /// </summary>
        /// <param name="team">Time</param>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        /// <returns></returns>
        public int getPlayingPlayers(int team, SLOT_STATE state, int type)
        {
            int players = 0;
            lock (_slots)
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = _slots[i];
                    if (slot._playerId > 0 && (type == 0 && slot.state == state || type == 1 && slot.state > state) && (team == 2 || slot._team == team))
                        players++;
                }
            return players;
        }
        /// <summary>
        /// Retorna a quantidade de jogadores, seguindo alguns parâmetros.
        /// </summary>
        /// <param name="team">Time</param>
        /// <param name="state">Estado do slot</param>
        /// <param name="type">Tipo de pesquisa. 0 = Igual; 1 = Maior</param>
        /// <param name="exception">Slot de excessão</param>
        /// <returns></returns>
        public int getPlayingPlayers(int team, SLOT_STATE state, int type, int exception)
        {
            int players = 0;
            lock (_slots)
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = _slots[i];
                    if (i != exception && slot._playerId > 0 && (type == 0 && slot.state == state || type == 1 && slot.state > state) && (team == 2 || slot._team == team))
                        players++;
                }
            return players;
        }
        /// <summary>
        /// Se inBattle for TRUE pega slots com "state" = BATTLE e "espectador" = FALSE.
        /// <para>Se inBattle for FALSE pega slots com "state" >= RENDEZVOUS.</para>
        /// </summary>
        /// <param name="inBattle">Pegar jogadores em partida?</param>
        /// <param name="RedPlayers">Quantidade (Time vermelho)</param>
        /// <param name="BluePlayers">Quantidade (Time azul)</param>
        public void getPlayingPlayers(bool inBattle, out int RedPlayers, out int BluePlayers)
        {
            RedPlayers = 0;
            BluePlayers = 0;
            lock (_slots)
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = _slots[i];
                    if (slot._playerId > 0 && (inBattle && (int)slot.state == 13 && !slot.espectador || !inBattle && (int)slot.state >= 10))
                        if (slot._team == 0) RedPlayers++;
                        else BluePlayers++;
                }
        }
        public void getPlayingPlayers(bool inBattle, out int RedPlayers, out int BluePlayers, out int RedDeaths, out int BlueDeaths)
        {
            RedPlayers = 0;
            BluePlayers = 0;
            RedDeaths = 0;
            BlueDeaths = 0;
            lock (_slots)
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = _slots[i];
                    if (slot._deathState.HasFlag(DeadEnum.isDead))
                        if (slot._team == 0) RedDeaths++;
                        else BlueDeaths++;
                    if (slot._playerId > 0 && (inBattle && (int)slot.state == 13 && !slot.espectador || !inBattle && (int)slot.state >= 10))
                        if (slot._team == 0) RedPlayers++;
                        else BluePlayers++;
                }
        }
        public void CheckToEndWaitingBattle(bool host)
        {
            //Logger.warning("state: " + _roomState + "; leader: " + _slots[_leader].state);
            if ((_state == RoomState.CountDown || _state == RoomState.Loading || _state == RoomState.Rendezvous) && (host || _slots[_leader].state == SLOT_STATE.BATTLE_READY))
            {
                AllUtils.EndBattleNoPoints(this);
            }
        }
        /// <summary>
        /// Tenta iniciar a partida para todos os jogadores que estiverem com o estado BATTLE_READY. Chama o updateSlotsInfo e updateRoomInfo.
        /// </summary>
        public void SpawnReadyPlayers()
        {
            lock (_slots)
                BaseSpawnReadyPlayers(isBotMode());
        }
        /// <summary>
        /// Tenta iniciar a partida para todos os jogadores que estiverem com o estado BATTLE_READY. Chama o updateSlotsInfo e updateRoomInfo.
        /// </summary>
        public void SpawnReadyPlayers(bool isBotMode)
        {
            lock (_slots)
                BaseSpawnReadyPlayers(isBotMode);
        }
        private void BaseSpawnReadyPlayers(bool isBotMode)
        {
            DateTime date = DateTime.Now;
            for (int i = 0; i < 16; i++)
            {
                SLOT slot = _slots[i];
                if ((int)slot.state == 12 && slot.isPlaying == 0 && slot._playerId > 0)
                {
                    slot.isPlaying = 1;
                    slot.startTime = date;
                    slot.state = SLOT_STATE.BATTLE;
                    if (_state == RoomState.Battle && (room_type == 2 || room_type == 4))
                        slot.espectador = true;
                }
            }
            updateSlotsInfo();
            List<int> dinos = AllUtils.getDinossaurs(this, false, -1);
            if (_state == RoomState.PreBattle)
            {
                BattleStart = room_type == 7 || room_type == 12 ? date.AddMinutes(5) : date;
                SetSpecialStage();
            }
            bool dinoStart = false;
            using (BATTLE_ROUND_RESTART_PAK packet = new BATTLE_ROUND_RESTART_PAK(this, dinos, isBotMode))
            using (BATTLE_TIMERSYNC_PAK packet2 = new BATTLE_TIMERSYNC_PAK(this))
            using (BATTLE_RECORD_PAK packet3 = new BATTLE_RECORD_PAK(this))
            {
                byte[] data = packet.GetCompleteBytes("Room.BaseSpawnReadyPlayers-1");
                byte[] data2 = packet2.GetCompleteBytes("Room.BaseSpawnReadyPlayers-2");
                byte[] data3 = packet3.GetCompleteBytes("Room.BaseSpawnReadyPlayers-3");
                for (int i = 0; i < 16; i++)
                {
                    Account player;
                    SLOT slot = _slots[i];
                    if ((int)slot.state == 13 && slot.isPlaying == 1 && getPlayerBySlot(slot, out player))
                    {
                        slot.isPlaying = 2;
                        if (_state == RoomState.PreBattle)
                        {
                            using (BATTLE_STARTBATTLE_PAK packet4 = new BATTLE_STARTBATTLE_PAK(slot, player, dinos, isBotMode, true))
                                SendPacketToPlayers(packet4, SLOT_STATE.READY, 1);
                            player.SendCompletePacket(data);
                            if (room_type == 7 || room_type == 12)
                                dinoStart = true;
                            else
                                player.SendCompletePacket(data2);
                        }
                        else if (_state == RoomState.Battle)
                        {
                            using (BATTLE_STARTBATTLE_PAK packet4 = new BATTLE_STARTBATTLE_PAK(slot, player, dinos, isBotMode, false))
                                SendPacketToPlayers(packet4, SLOT_STATE.READY, 1);
                            if (room_type == 2 || room_type == 4)
                                Game_SyncNet.SendUDPPlayerSync(this, slot, 0, 1);
                            else
                                player.SendCompletePacket(data);
                            player.SendCompletePacket(data2);
                            player.SendCompletePacket(data3);
                        }
                    }
                }
            }
            if (_state == RoomState.PreBattle)
            {
                _state = RoomState.Battle;
                updateRoomInfo();
            }
            if (dinoStart)
                StartDinoRound();
        }
        private void StartDinoRound()
        {
            round.StartJob(5250, (callbackState) =>
            {
                if (_state == RoomState.Battle)
                {
                    using (BATTLE_TIMERSYNC_PAK packet = new BATTLE_TIMERSYNC_PAK(this))
                        SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                    swapRound = false;
                }
                lock (callbackState)
                { round.Timer = null; }
            });
        }
    }
}
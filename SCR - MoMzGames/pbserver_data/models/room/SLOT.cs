/*
 * Arquivo: SLOT.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 16/12/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.players;
using Core.models.enums;
using Core.models.enums.flags;
using Core.server;
using System;
using System.Collections.Generic;

namespace Core.models.room
{
    public class SLOT
    {
        public SLOT_STATE state;
        public ResultIcon bonusFlags;
        public PlayerEquipedItems _equip;
        public long _playerId;
        public DeadEnum _deathState = DeadEnum.isAlive;
        public bool firstRespawn = true, repeatLastState, check, espectador, specGM, withHost;
        public int _id, _team, _flag, aiLevel, latency, failLatencyTimes, ping, passSequence, isPlaying, earnedXP, spawnsCount, headshots, lastKillState, killsOnLife, exp, money, gp, Score, allKills, allDeaths, objetivos, BonusXP, BonusGP,
            unkItem;
        public DateTime NextVoteDate, startTime, preStartDate, preLoadDate;
        public ushort damageBar1, damageBar2;
        public List<int> armas_usadas = new List<int>();
        public bool MissionsCompleted;
        public PlayerMissions Missions;
        public TimerState timing = new TimerState();
        /// <summary>
        /// Cancela a contagem de tempo para o jogador ser expulso da partida.
        /// </summary>
        public void StopTiming()
        {
            if (timing != null)
                timing.Timer = null;
        }
        public SLOT(int slotIdx)
        {
            SetSlotId(slotIdx);
        }
        public void SetSlotId(int slotIdx)
        {
            _id = slotIdx;
            _team = (slotIdx % 2);
            _flag = (1 << slotIdx);
        }
        public void ResetSlot()
        {
            repeatLastState = false;
            _deathState = DeadEnum.isAlive;
            StopTiming();
            check = false;
            espectador = false;
            specGM = false;
            withHost = false;
            firstRespawn = true;
            failLatencyTimes = 0;
            latency = 0;
            ping = 0;
            passSequence = 0;
            allDeaths = 0;
            allKills = 0;
            bonusFlags = 0;
            killsOnLife = 0;
            lastKillState = 0;
            Score = 0;
            gp = 0;
            exp = 0;
            headshots = 0;
            objetivos = 0;
            BonusGP = 0;
            BonusXP = 0;
            spawnsCount = 0;
            damageBar1 = 0;
            damageBar2 = 0;
            earnedXP = 0;
            isPlaying = 0;
            money = 0;
            NextVoteDate = new DateTime();
            aiLevel = 0;
            armas_usadas.Clear();
            MissionsCompleted = false;
            Missions = null;
        }
        public void SetMissionsClone(PlayerMissions missions)
        {
            Missions = missions.DeepCopy();
            MissionsCompleted = false;
        }
        /// <summary>
        /// Retorna a quantidade de segundos em que o jogador participou da partida.
        /// </summary>
        /// <param name="date">Data de término da partida</param>
        /// <returns></returns>
        public double inBattleTime(DateTime date)
        {
            if (startTime == new DateTime())// || startTime > date)
                return 0;
            return (date - startTime).TotalSeconds;
        }
    }
}
/*
 * Arquivo: BATTLE_PLAYER_LEAVE_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/10/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.models.enums;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class BATTLE_PLAYER_LEAVE_REC : ReceiveGamePacket
    {
        private bool isFinished;
        private long objId;
        public BATTLE_PLAYER_LEAVE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            objId = readQ(); //O bom perdedor (Unidade) - Se ele Usar o item retorna o OBJ do item.
            //0x18E21C00000000
        }
        public override void run()
        {
            try
            {
                Account p = _client._player;
                Room room = p == null ? null : p._room;
                SLOT slot;
                if (room != null && (int)room._state >= 2 && room.getSlot(p._slotId, out slot) && (int)slot.state >= 9)
                {
                    bool isBotMode = room.isBotMode();
                    FreepassEffect(p, slot, room, isBotMode);
                    if (room.vote.Timer != null && room.votekick != null && room.votekick.victimIdx == slot._id)
                    {
                        room.vote.Timer = null;
                        room.votekick = null;
                        using (VOTEKICK_CANCEL_VOTE_PAK packet = new VOTEKICK_CANCEL_VOTE_PAK())
                            room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0, slot._id);
                    }
                    AllUtils.ResetSlotInfo(room, slot, true);
                    int red13 = 0, blue13 = 0, red9 = 0, blue9 = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        SLOT slotR = room._slots[i];
                        if ((int)slotR.state >= 9)
                        {
                            if (slotR._team == 0) red9++;
                            else blue9++;
                            if (slotR.state == SLOT_STATE.BATTLE)
                            {
                                if (slotR._team == 0) red13++;
                                else blue13++;
                            }
                        }
                    }
                    if (slot._id == room._leader)
                    {
                        if (isBotMode)
                        {
                            if (red13 > 0 || blue13 > 0)
                                LeaveHostBOT_GiveBattle(room, p);
                            else
                                LeaveHostBOT_EndBattle(room, p);
                        }
                        else if ((int)room._state == 5 && (red13 == 0 || blue13 == 0) || (int)room._state <= 4 && (red9 == 0 || blue9 == 0))
                            LeaveHostNoBOT_EndBattle(room, p, red13, blue13);
                        else
                            LeaveHostNoBOT_GiveBattle(room, p);
                    }
                    else if (!isBotMode)
                    {
                        if ((int)room._state == 5 && (red13 == 0 || blue13 == 0) || (int)room._state <= 4 && (red9 == 0 || blue9 == 0))
                            LeavePlayerNoBOT_EndBattle(room, p, red13, blue13);
                        else
                            LeavePlayer_QuitBattle(room, p);
                    }
                    else
                        LeavePlayer_QuitBattle(room, p);
                    _client.SendPacket(new BATTLE_LEAVEP2PSERVER_PAK(p, 0));
                    if (!isFinished && room._state == RoomState.Battle)
                        AllUtils.BattleEndRoundPlayersCount(room);
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[BATTLE_PLAYER_LEAVE_REC] " + ex.ToString());
            }
        }
        private void FreepassEffect(Account p, SLOT slot, Room room, bool isBotMode)
        {
            DBQuery query = new DBQuery();
            if (p._bonus.freepass == 0 || p._bonus.freepass == 1 && room._channelType == 4)
            {
                if (isBotMode || slot.state < SLOT_STATE.BATTLE_READY)
                    return;

                if (p._gp > 0)
                {
                    p._gp -= 200;
                    if (p._gp < 0) p._gp = 0;
                    query.AddQuery("gp", p._gp);
                }
                query.AddQuery("escapes", ++p._statistic.escapes);
            }
            else// if (ch._type != 4)
            {
                if (room._state != RoomState.Battle)
                    return;
                int xp = 0, gp = 0;
                if (isBotMode)
                {
                    int level = room.IngameAiLevel * (150 + slot.allDeaths);
                    if (level == 0)
                        level++;
                    int reward = (slot.Score / level);
                    gp += reward;
                    xp += reward;
                }
                else
                {
                    int timePlayed = slot.allKills == 0 && slot.allDeaths == 0 ? 0 : (int)slot.inBattleTime(DateTime.Now);
                    if (room.room_type == 2 || room.room_type == 4)
                    {
                        xp = (int)(slot.Score + (timePlayed / 2.5) + (slot.allDeaths * 2.2) + (slot.objetivos * 20));
                        gp = (int)(slot.Score + (timePlayed / 3.0) + (slot.allDeaths * 2.2) + (slot.objetivos * 20));
                    }
                    else
                    {
                        xp = (int)(slot.Score + (timePlayed / 2.5) + (slot.allDeaths * 1.8) + (slot.objetivos * 20));
                        gp = (int)(slot.Score + (timePlayed / 3.0) + (slot.allDeaths * 1.8) + (slot.objetivos * 20));
                    }
                }
                p._exp += ConfigGS.maxBattleXP < xp ? ConfigGS.maxBattleXP : xp;
                p._gp += ConfigGS.maxBattleGP < gp ? ConfigGS.maxBattleGP : gp;
                if (gp > 0)
                    query.AddQuery("gp", p._gp);
                if (xp > 0)
                    query.AddQuery("exp", p._exp);
            }
            ComDiv.updateDB("accounts", "player_id", p.player_id, query.GetTables(), query.GetValues());
        }

        private void LeaveHostBOT_GiveBattle(Room room, Account p)
        {
            List<Account> players = room.getAllPlayers(SLOT_STATE.READY, 1);
            if (players.Count == 0)
                return;
            int oldLeader = room._leader;
            room.setNewLeader(-1, 12, room._leader, true);
            using (BATTLE_LEAVEP2PSERVER_PAK packet = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
            using (BATTLE_GIVEUPBATTLE_PAK packet2 = new BATTLE_GIVEUPBATTLE_PAK(room, oldLeader))
            {
                byte[] data = packet.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-1");
                byte[] data2 = packet2.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-2");
                foreach (Account pR in players)
                {
                    SLOT slot = room.getSlot(pR._slotId);
                    if (slot != null)
                    {
                        if (slot.state >= SLOT_STATE.PRESTART)
                            pR.SendCompletePacket(data2);
                        pR.SendCompletePacket(data);
                    }
                }
            }
        }
        private void LeaveHostBOT_EndBattle(Room room, Account p)
        {
            List<Account> players = room.getAllPlayers(SLOT_STATE.READY, 1);
            if (players.Count == 0)
                goto EndLabel;
            using (BATTLE_LEAVEP2PSERVER_PAK packet = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
            {
                byte[] data = packet.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-3");
                TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room);
                ushort inBattle, missionCompletes;
                byte[] a1;
                AllUtils.getBattleResult(room, out missionCompletes, out inBattle, out a1);
                foreach (Account pR in players)
                {
                    pR.SendCompletePacket(data);
                    pR.SendPacket(new BATTLE_ENDBATTLE_PAK(pR, winnerTeam, inBattle, missionCompletes, true, a1));
                }
            }
        EndLabel:
            AllUtils.resetBattleInfo(room);
        }
        /// <summary>
        /// Falta de usuários para continuar com a partida em andamento.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="p"></param>
        /// <param name="oldLeader"></param>
        private void LeaveHostNoBOT_EndBattle(Room room, Account p, int red13, int blue13)
        {
            isFinished = true;
            List<Account> players = room.getAllPlayers(SLOT_STATE.READY, 1);
            if (players.Count == 0)
                goto EndLabel;
            TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room, red13, blue13);
            if (room._state == RoomState.Battle)
                room.CalculateResult(winnerTeam, false);
            using (BATTLE_LEAVEP2PSERVER_PAK packet = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
            {
                byte[] data = packet.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-4");
                ushort inBattle, missionCompletes;
                byte[] a1;
                AllUtils.getBattleResult(room, out missionCompletes, out inBattle, out a1);
                foreach (Account pR in players)
                {
                    pR.SendCompletePacket(data);
                    pR.SendPacket(new BATTLE_ENDBATTLE_PAK(pR, winnerTeam, inBattle, missionCompletes, false, a1));
                }
            }
        EndLabel:
            AllUtils.resetBattleInfo(room);
        }
        private void LeaveHostNoBOT_GiveBattle(Room room, Account p)
        {
            List<Account> players = room.getAllPlayers(SLOT_STATE.READY, 1);
            if (players.Count == 0)
                return;
            int oldLeader = room._leader;
            int state = (room._state == RoomState.Battle ? 12 : 8);
            room.setNewLeader(-1, state, room._leader, true);
            using (BATTLE_GIVEUPBATTLE_PAK packet = new BATTLE_GIVEUPBATTLE_PAK(room, oldLeader))
            using (BATTLE_LEAVEP2PSERVER_PAK packet2 = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
            {
                byte[] data = packet.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-6");
                byte[] data2 = packet2.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-7");
                foreach (Account pR in players)
                {
                    if (room._slots[pR._slotId].state >= SLOT_STATE.PRESTART)
                        pR.SendCompletePacket(data);
                    pR.SendCompletePacket(data2);
                }
            }
        }
        private void LeavePlayerNoBOT_EndBattle(Room room, Account p, int red13, int blue13)
        {
            isFinished = true;
            TeamResultType winnerTeam = AllUtils.GetWinnerTeam(room, red13, blue13);
            List<Account> players = room.getAllPlayers(SLOT_STATE.READY, 1);
            if (players.Count == 0)
                goto EndLabel;
            if (room._state == RoomState.Battle)
                room.CalculateResult(winnerTeam, false);
            using (BATTLE_LEAVEP2PSERVER_PAK packet = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
            {
                byte[] data = packet.GetCompleteBytes("BATTLE_PLAYER_LEAVE_REC-8");
                ushort inBattle, missionCompletes;
                byte[] a1;
                AllUtils.getBattleResult(room, out missionCompletes, out inBattle, out a1);
                foreach (Account pR in players)
                {
                    pR.SendCompletePacket(data);
                    pR.SendPacket(new BATTLE_ENDBATTLE_PAK(pR, winnerTeam, inBattle, missionCompletes, false, a1));
                }
            }
        EndLabel:
            AllUtils.resetBattleInfo(room);
        }
        private void LeavePlayer_QuitBattle(Room room, Account p)
        {
            using (BATTLE_LEAVEP2PSERVER_PAK packet = new BATTLE_LEAVEP2PSERVER_PAK(p, 0))
                room.SendPacketToPlayers(packet, SLOT_STATE.READY, 1);
        }
    }
}
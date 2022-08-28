/*
 * Arquivo: BATTLE_READYBATTLE_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 17/11/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.models.account.players;
using Core.models.enums;
using Core.models.room;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class BATTLE_READYBATTLE_REC : ReceiveGamePacket
    {
        private int erro;
        public BATTLE_READYBATTLE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            erro = readD(); //0 - NORMAL || 1 - OBSERVER (GM)
        }
        private bool ClassicModeCheck(Account p, Room room)
        {
            if (!room.name.ToLower().Contains("@camp") && !room.name.ToLower().Contains("camp") &&
                !room.name.ToLower().Contains("@cnpb") && !room.name.ToLower().Contains("cnpb") &&
                !room.name.ToLower().Contains("@79") && !room.name.ToLower().Contains("79") &&
                !room.name.ToLower().Contains("@Lan") && !room.name.ToLower().Contains("@lan")// &&
                )
                return false;
            List<string> blocks = new List<string>();
            PlayerEquipedItems equip = p._equip;

            //Bloqueio Camp
            if (room.name.ToLower().Contains("@camp") ||
                room.name.ToLower().Contains(" @camp")||
                room.name.ToLower().Contains("@camp ")||
                room.name.ToLower().Contains("camp")
                )
            {
                for (int i = 0; i < ClassicModeManager.itemscamp.Count; i++)
                {
                    int id = ClassicModeManager.itemscamp[i];
                    if (ClassicModeManager.IsBlocked(id, equip._primary, ref blocks, Translation.GetLabel("ClassicCategory1")) ||
                    ClassicModeManager.IsBlocked(id, equip._secondary, ref blocks, Translation.GetLabel("ClassicCategory2")) ||
                    ClassicModeManager.IsBlocked(id, equip._melee, ref blocks, Translation.GetLabel("ClassicCategory3")) ||
                    ClassicModeManager.IsBlocked(id, equip._grenade, ref blocks, Translation.GetLabel("ClassicCategory4")) ||
                    ClassicModeManager.IsBlocked(id, equip._special, ref blocks, Translation.GetLabel("ClassicCategory5")) ||
                    ClassicModeManager.IsBlocked(id, equip._red, ref blocks, Translation.GetLabel("ClassicCategory6")) ||
                    ClassicModeManager.IsBlocked(id, equip._blue, ref blocks, Translation.GetLabel("ClassicCategory7")) ||
                    ClassicModeManager.IsBlocked(id, equip._helmet, ref blocks, Translation.GetLabel("ClassicCategory8")) ||
                    ClassicModeManager.IsBlocked(id, equip._dino, ref blocks, Translation.GetLabel("ClassicCategory9")) ||
                    ClassicModeManager.IsBlocked(id, equip._beret, ref blocks, Translation.GetLabel("ClassicCategory10")))
                        continue;
                }
            }
            //Bloqueio CNPB
            if (room.name.ToLower().Contains("@cnpb") ||
                room.name.ToLower().Contains("@cnpb ")|| 
                room.name.ToLower().Contains(" @cnpb")||
                room.name.ToLower().Contains("cnpb")
                )
            {
                for (int i = 0; i < ClassicModeManager.itemscnpb.Count; i++)
                {
                    int id = ClassicModeManager.itemscnpb[i];
                    if (ClassicModeManager.IsBlocked(id, equip._primary, ref blocks, Translation.GetLabel("ClassicCategory1")) ||
                    ClassicModeManager.IsBlocked(id, equip._secondary, ref blocks, Translation.GetLabel("ClassicCategory2")) ||
                   ClassicModeManager.IsBlocked(id, equip._melee, ref blocks, Translation.GetLabel("ClassicCategory3")) ||
                   ClassicModeManager.IsBlocked(id, equip._grenade, ref blocks, Translation.GetLabel("ClassicCategory4")) ||
                   ClassicModeManager.IsBlocked(id, equip._special, ref blocks, Translation.GetLabel("ClassicCategory5")) ||
                   ClassicModeManager.IsBlocked(id, equip._red, ref blocks, Translation.GetLabel("ClassicCategory6")) ||
                   ClassicModeManager.IsBlocked(id, equip._blue, ref blocks, Translation.GetLabel("ClassicCategory7")) ||
                   ClassicModeManager.IsBlocked(id, equip._helmet, ref blocks, Translation.GetLabel("ClassicCategory8")) ||
                   ClassicModeManager.IsBlocked(id, equip._dino, ref blocks, Translation.GetLabel("ClassicCategory9")) ||
                   ClassicModeManager.IsBlocked(id, equip._beret, ref blocks, Translation.GetLabel("ClassicCategory10")))
                        continue;
                }
            }
            //Bloqueio 79
            if (room.name.ToLower().Contains("@79")||
                room.name.ToLower().Contains("@79" )||
                room.name.ToLower().Contains(" @79")||
                room.name.ToLower().Contains("79")
                )
            {
                for (int i = 0; i < ClassicModeManager.items79.Count; i++)
                {
                    int id = ClassicModeManager.items79[i];
                    if (ClassicModeManager.IsBlocked(id, equip._primary, ref blocks, Translation.GetLabel("ClassicCategory1")) ||
                    ClassicModeManager.IsBlocked(id, equip._secondary, ref blocks, Translation.GetLabel("ClassicCategory2")) ||
                   ClassicModeManager.IsBlocked(id, equip._melee, ref blocks, Translation.GetLabel("ClassicCategory3")) ||
                   ClassicModeManager.IsBlocked(id, equip._grenade, ref blocks, Translation.GetLabel("ClassicCategory4")) ||
                   ClassicModeManager.IsBlocked(id, equip._special, ref blocks, Translation.GetLabel("ClassicCategory5")) ||
                   ClassicModeManager.IsBlocked(id, equip._red, ref blocks, Translation.GetLabel("ClassicCategory6")) ||
                   ClassicModeManager.IsBlocked(id, equip._blue, ref blocks, Translation.GetLabel("ClassicCategory7")) ||
                   ClassicModeManager.IsBlocked(id, equip._helmet, ref blocks, Translation.GetLabel("ClassicCategory8")) ||
                   ClassicModeManager.IsBlocked(id, equip._dino, ref blocks, Translation.GetLabel("ClassicCategory9")) ||
                   ClassicModeManager.IsBlocked(id, equip._beret, ref blocks, Translation.GetLabel("ClassicCategory10")))
                        continue;
                }
            }
            //Bloqeuio Lan
            if (room.name.ToLower().Contains("@lan") ||
                room.name.ToLower().Contains("@Lan")
                )
            {
                for (int i = 0; i < ClassicModeManager.itemslan.Count; i++)
                {
                    int id = ClassicModeManager.itemslan[i];
                    if (ClassicModeManager.IsBlocked(id, equip._primary, ref blocks, Translation.GetLabel("ClassicCategory1")) ||
                    ClassicModeManager.IsBlocked(id, equip._secondary, ref blocks, Translation.GetLabel("ClassicCategory2")) ||
                    ClassicModeManager.IsBlocked(id, equip._melee, ref blocks, Translation.GetLabel("ClassicCategory3")) ||
                    ClassicModeManager.IsBlocked(id, equip._grenade, ref blocks, Translation.GetLabel("ClassicCategory4")) ||
                    ClassicModeManager.IsBlocked(id, equip._special, ref blocks, Translation.GetLabel("ClassicCategory5")) ||
                    ClassicModeManager.IsBlocked(id, equip._red, ref blocks, Translation.GetLabel("ClassicCategory6")) ||
                    ClassicModeManager.IsBlocked(id, equip._blue, ref blocks, Translation.GetLabel("ClassicCategory7")) ||
                    ClassicModeManager.IsBlocked(id, equip._helmet, ref blocks, Translation.GetLabel("ClassicCategory8")) ||
                    ClassicModeManager.IsBlocked(id, equip._dino, ref blocks, Translation.GetLabel("ClassicCategory9")) ||
                    ClassicModeManager.IsBlocked(id, equip._beret, ref blocks, Translation.GetLabel("ClassicCategory10")))
                        continue;
                }
            }

            if (blocks.Count > 0)
            {
                p.SendPacket(new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("ClassicModeWarn", string.Join(", ", blocks.ToArray()))));
                return true;
            }
            return false;
        }
        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                    return;
                Room room = player._room;
                Channel ch;
                SLOT slot;
                if (room == null || room.getLeader() == null || !room.getChannel(out ch) || !room.getSlot(player._slotId, out slot))
                    return;
                if (slot._equip == null)
                {//STBL_IDX_EP_ROOM_ERROR_READY_WEAPON_EQUIP
                    _client.SendPacket(new BATTLE_READY_ERROR_PAK(0x800010AB));
                    return;
                }
                bool isBotMode = room.isBotMode();
                if (erro == 1 && player.IsGM())
                    slot.specGM = true;
                else
                    slot.specGM = false;
                player.DebugPing = false;
                if (ConfigGS.EnableClassicRules && ClassicModeCheck(player, room))
                    return;
                if (room._leader == player._slotId)
                {
                    if (room._state != RoomState.Ready && room._state != RoomState.CountDown)
                        return;
                    int TotalEnemys = 0, redPlayers = 0, bluePlayers = 0;
                    GetReadyPlayers(room, ref redPlayers, ref bluePlayers, ref TotalEnemys);
                    if (ConfigGS.isTestMode && ConfigGS.udpType == SERVER_UDP_STATE.RELAY)
                        TotalEnemys = 1;
                    if (room.stage4v4 == 1 && (redPlayers >= 4 || bluePlayers >= 4))
                    {
                        _client.SendPacket(new BATTLE_4VS4_ERROR_PAK());
                        return;
                    }
                    if (ClanMatchCheck(room, ch._type, TotalEnemys))
                        return;
                    if (isBotMode || room.room_type == 10 || TotalEnemys > 0 && !isBotMode)
                    {
                        room.changeSlotState(slot, SLOT_STATE.READY, false);
                        if ((int)room._state != 1)
                            TryBalanceTeams(room, isBotMode);
                        if (room.thisModeHaveCD())
                        {
                            if ((int)room._state == 0)
                            {
                                room._state = RoomState.CountDown;
                                room.updateRoomInfo();
                                room.StartCountDown();
                            }
                            else if ((int)room._state == 1)
                            {
                                room.changeSlotState(room._leader, SLOT_STATE.NORMAL, false);
                                room.StopCountDown(CountDownEnum.StopByHost);
                            }
                        }
                        else
                            room.StartBattle(false);
                        room.updateSlotsInfo();
                    }
                    else if (TotalEnemys == 0 && !isBotMode)
                        _client.SendPacket(new BATTLE_READY_ERROR_PAK(0x80001009));
                }
                else if ((int)room._slots[room._leader].state >= 9)
                {
                    if (slot.state == SLOT_STATE.NORMAL)
                    {
                        if (room.autobalans == 1 && !isBotMode)
                            AllUtils.TryBalancePlayer(room, player, true, ref slot);
                        room.changeSlotState(slot, SLOT_STATE.LOAD, true);
                        slot.SetMissionsClone(player._mission);
                        _client.SendPacket(new BATTLE_READYBATTLE_PAK(room));
                        _client.SendPacket(new BATTLE_READY_ERROR_PAK((uint)slot.state));
                        using (BATTLE_READYBATTLE2_PAK packet = new BATTLE_READYBATTLE2_PAK(slot, player._titles))
                            room.SendPacketToPlayers(packet, SLOT_STATE.READY, 1, slot._id);
                    }
                }
                else if ((int)slot.state == 7)
                {
                    room.changeSlotState(slot, SLOT_STATE.READY, true);
                    if ((int)room._state == 1)
                        TryBalanceTeams(room, isBotMode);
                }
                else if ((int)slot.state == 8)
                {
                    room.changeSlotState(slot, SLOT_STATE.NORMAL, false);
                    if ((int)room._state == 1 && room.getPlayingPlayers(room._leader % 2 == 0 ? 1 : 0, SLOT_STATE.READY, 0) == 0)
                    {
                        room.changeSlotState(room._leader, SLOT_STATE.NORMAL, false);
                        room.StopCountDown(CountDownEnum.StopByPlayer);
                    }
                    room.updateSlotsInfo();
                }
            }
            catch (Exception ex)
            {
                Logger.info("BATTLE_READYBATTLE_REC: " + ex.ToString());
            }
        }
        private void GetReadyPlayers(Room room, ref int redPlayers, ref int bluePlayers, ref int TotalEnemys)
        {
            for (int i = 0; i < 16; i++)
            {
                SLOT slot = room._slots[i];
                if ((int)slot.state == 8)
                {
                    if (slot._team == 0)
                        redPlayers++;
                    else
                        bluePlayers++;
                }
            }
            if (room._leader % 2 == 0)
                TotalEnemys = bluePlayers;
            else
                TotalEnemys = redPlayers;
        }
        private bool ClanMatchCheck(Room room, int type, int TotalEnemys)
        {
            if (ConfigGS.isTestMode || type != 4)
                return false;

            if (!AllUtils.Have2ClansToClanMatch(room))
            {//STBL_IDX_EP_ROOM_NO_START_FOR_NO_CLAN_TEAM
                _client.SendPacket(new BATTLE_READY_ERROR_PAK(0x80001071));
                return true;
            }

            if (TotalEnemys > 0 && !AllUtils.HavePlayersToClanMatch(room))
            {//STBL_IDX_EP_ROOM_NO_START_FOR_TEAM_NOTFULL
                _client.SendPacket(new BATTLE_READY_ERROR_PAK(0x80001072));
                return true;
            }
            return false;
        }
        private void TryBalanceTeams(Room room, bool isBotMode)
        {
            if (room.autobalans != 1 || isBotMode)
                return;
            int TeamIdx = AllUtils.getBalanceTeamIdx(room, false, -1);
            if (TeamIdx == -1)
                return;
            int[] teamArray = TeamIdx == 1 ? room.RED_TEAM : room.BLUE_TEAM;
            SLOT LastSlot = null;
            for (int i = teamArray.Length - 1; i >= 0; i--)
            {
                SLOT slot = room._slots[teamArray[i]];
                if ((int)slot.state == 8 && room._leader != slot._id)
                {
                    LastSlot = slot;
                    break;
                }
            }
            Account player;
            if (LastSlot != null && room.getPlayerBySlot(LastSlot, out player))
                AllUtils.TryBalancePlayer(room, player, false, ref LastSlot);
        }
    }
}
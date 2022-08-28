using Core;
using Core.models.enums;
using Core.models.enums.errors;
using Core.models.room;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_PRESTARTBATTLE_REC : ReceiveGamePacket
    {
        private int mapId, stage4v4, room_type;
        public BATTLE_PRESTARTBATTLE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            mapId = readH();
            stage4v4 = readC();
            room_type = readC();
        }
        private int genValor(int first, int second)
        {
            return (first * 16) + second;
        }
        public override void run()
        {
            try
            {
                if (_client == null)
                    return;
                Account p = _client._player;
                Room room = p == null ? null : p._room;
                if (room != null && (room.stage4v4 == stage4v4 && room.room_type == room_type && room.mapId == mapId))
                {
                    SLOT slot = room._slots[p._slotId];
                    if (room.isPreparing() && room.UDPServer != null && (int)slot.state >= 9)
                    {
                        Account leader = room.getLeader();
                        if (leader != null)
                        {
                            if (p.LocalIP == new byte[4] || string.IsNullOrEmpty(p.PublicIP.ToString()))
                            {
                                _client.SendPacket(new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_No_Real_IP));
                                _client.SendPacket(new BATTLE_LEAVEP2PSERVER_PAK(p, 0));
                                room.changeSlotState(slot, SLOT_STATE.NORMAL, true);
                                AllUtils.BattleEndPlayersCount(room, room.isBotMode());
                                slot.StopTiming();
                                return;
                            }
                            int gen2 = genValor(room.mapId, room.room_type);
                            if (slot._id == room._leader)
                            {
                                room._state = RoomState.PreBattle;
                                room.updateRoomInfo();
                                //room.LoadHitParts();
                            }
                            slot.preStartDate = DateTime.Now;
                            room.StartCounter(1, p, slot);
                            room.changeSlotState(slot, SLOT_STATE.PRESTART, true);
                            _client.SendPacket(new BATTLE_PRESTARTBATTLE_PAK(p, leader, true, gen2));
                            if (slot._id != room._leader)
                                leader.SendPacket(new BATTLE_PRESTARTBATTLE_PAK(p, leader, false, gen2));

                            //slot.state = SLOT_STATE.BATTLE;
                            //room.updateSlotsInfo();
                        }
                        else
                        {
                            _client.SendPacket(new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_First_Hole));
                            _client.SendPacket(new BATTLE_LEAVEP2PSERVER_PAK(p, 0));
                            room.changeSlotState(slot, SLOT_STATE.NORMAL, true);
                            AllUtils.BattleEndPlayersCount(room, room.isBotMode());
                            slot.StopTiming();
                        }
                    }
                    else
                    {
                        room.changeSlotState(slot, SLOT_STATE.NORMAL, true);
                        _client.SendPacket(new BATTLE_STARTBATTLE_PAK());
                        AllUtils.BattleEndPlayersCount(room, room.isBotMode());
                        slot.StopTiming();
                    }
                }
                else
                {
                    _client.SendPacket(new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_First_MainLoad));
                    _client.SendPacket(new BATTLE_PRESTARTBATTLE_PAK());
                    if (room != null)
                    {
                        room.changeSlotState(p._slotId, SLOT_STATE.NORMAL, true);
                        AllUtils.BattleEndPlayersCount(room, room.isBotMode());
                    }
                    else
                        _client.SendPacket(new LOBBY_ENTER_PAK());
                }
            }
            catch (Exception ex)
            {
                Logger.info("BATTLE_PRESTARTBATTLE_REC: " + ex.ToString());
            }
        }
    }
}
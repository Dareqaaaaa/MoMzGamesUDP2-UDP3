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
    public class BATTLE_STARTBATTLE_REC : ReceiveGamePacket
    {
        public BATTLE_STARTBATTLE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                if (_client == null)
                    return;
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && room.isPreparing())
                {
                    bool isBotMode = room.isBotMode();
                    SLOT slot = room.getSlot(player._slotId);
                    if (slot == null)
                        return;
                    if (slot.state == SLOT_STATE.PRESTART)
                    {
                        room.changeSlotState(slot, SLOT_STATE.BATTLE_READY, true);
                        slot.StopTiming();
                        if (isBotMode)
                            _client.SendPacket(new BATTLE_CHANGE_DIFFICULTY_LEVEL_PAK(room));
                        _client.SendPacket(new BATTLE_ROOM_INFO_PAK(room, isBotMode)); //?
                    }
                    else
                    {
                        _client.SendPacket(new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_First_Hole));
                        _client.SendPacket(new BATTLE_LEAVEP2PSERVER_PAK(player, 0));
                        room.changeSlotState(slot, SLOT_STATE.NORMAL, true);
                        AllUtils.BattleEndPlayersCount(room, isBotMode);
                        return;
                    }
                    int blue12 = 0, red12 = 0, total = 0, red9 = 0, blue9 = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        SLOT slotR = room._slots[i];
                        if ((int)slotR.state >= 9)
                        {
                            total++;
                            if (slotR._team == 0) red9++;
                            else blue9++;
                            if ((int)slotR.state >= 12)
                            {
                                if (slotR._team == 0) red12++;
                                else blue12++;
                            }
                        }
                    }
                    /*
                    string txt = Translation.GetLabel("PlayerReady", player.player_name, (red + blue), total);
                    using (SERVER_MESSAGE_ANNOUNCE_PAK packet = new SERVER_MESSAGE_ANNOUNCE_PAK(txt))
                        room.SendPacketToPlayers(packet, SLOT_STATE.RENDEZVOUS, 1);*/
                    if ((int)room._state == 5 ||
                        (int)room._slots[room._leader].state >= 12 && isBotMode && 
                        (room._leader % 2 == 0 && red12 > red9 / 2 || room._leader % 2 == 1 && blue12 > blue9 / 2) ||

                        (int)room._slots[room._leader].state >= 12 && 

                        ((!ConfigGS.isTestMode || (int)ConfigGS.udpType != 3) &&
                        blue12 > blue9 / 2 && red12 > red9 / 2 ||
                        ConfigGS.isTestMode && (int)ConfigGS.udpType == 3))
                        room.SpawnReadyPlayers(isBotMode);
                }
                else
                {
                    _client.SendPacket(new SERVER_MESSAGE_KICK_BATTLE_PLAYER_PAK(EventErrorEnum.Battle_First_Hole));
                    _client.SendPacket(new BATTLE_STARTBATTLE_PAK());
                    if (room != null)
                        room.changeSlotState(player._slotId, SLOT_STATE.NORMAL, true);
                    if (room == null && player != null)
                        _client.SendPacket(new LOBBY_ENTER_PAK());
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[BATTLE_STARTBATTLE_REC] " + ex.ToString());
            }
        }
    }
}
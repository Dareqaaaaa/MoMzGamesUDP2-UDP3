/*
 * Arquivo: BATTLE_DEATH_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 15/11/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.models.enums;
using Core.models.enums.missions;
using Core.models.enums.room;
using Core.models.room;
using Game.data.model;
using Game.data.sync.client_side;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_DEATH_REC : ReceiveGamePacket
    {
        private FragInfos kills = new FragInfos();
        private bool isSuicide;
        public BATTLE_DEATH_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            kills.killingType = (CharaKillType)readC();
            kills.killsCount = readC();
            kills.killerIdx = readC();
            kills.weapon = readD();
            kills.x = readT();
            kills.y = readT();
            kills.z = readT();
            kills.flag = readC();
            for (int i = 0; i < kills.killsCount; i++)
            {
                Frag frag = new Frag();
                frag.victimWeaponClass = readC();
                frag.SetHitspotInfo(readC());
                readH();
                frag.flag = readC();
                frag.x = readT();
                frag.y = readT();
                frag.z = readT();
                kills.frags.Add(frag);
                if (frag.VictimSlot == kills.killerIdx)
                    isSuicide = true;
            }
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                    return;
                Room room = player._room;
                if (room == null || room.round.Timer != null || room._state < RoomState.Battle)
                    return;
                bool isBotMode = room.isBotMode();
                SLOT killer = room.getSlot(kills.killerIdx);
                if (killer == null || !isBotMode && (killer.state < SLOT_STATE.BATTLE || killer._id != player._slotId))
                    return;
                int score;
                Net_Room_Death.RegistryFragInfos(room, killer, out score, isBotMode, isSuicide, kills);
                if (isBotMode)
                {
                    killer.Score += killer.killsOnLife + room.IngameAiLevel + score;
                    if (killer.Score > 65535)
                    {
                        killer.Score = 65535;
                        Logger.LogHack("[Player: " + player.player_name + "; Id: " + _client.player_id + "] chegou a pontuação máxima do modo BOT.");
                    }
                    kills.Score = killer.Score;
                }
                else
                {
                    killer.Score += score;
                    AllUtils.CompleteMission(room, player, killer, kills, MISSION_TYPE.NA, 0);
                    kills.Score = score;
                }
                using (BATTLE_DEATH_PAK packet = new BATTLE_DEATH_PAK(room, kills, killer, isBotMode))
                    room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                Net_Room_Death.EndBattleByDeath(room, killer, isBotMode, isSuicide);
            }
            catch (Exception ex)
            {
                Logger.info("BATTLE_DEATH_REC: " + ex.ToString());
                _client.Close(0);
            }
        }
    }
}
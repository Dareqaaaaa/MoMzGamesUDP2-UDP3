using Core;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.enums.item;
using Core.models.enums.missions;
using Core.models.enums.room;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.data.xml;
using System;

namespace Game.data.sync.client_side
{
    public static class Net_Room_Death
    {
        public static void Load(ReceiveGPacket p)
        {
            int roomId = p.readH();
            int channelId = p.readH();
            byte killerId = p.readC();
            byte dieObjectId = p.readC();
            int weaponId = p.readD();
            float killerX = p.readT();
            float killerY = p.readT();
            float killerZ = p.readT();
            byte killsCount = p.readC();
            int estimado = (killsCount * 15);
            if (p.getBuffer().Length > (25 + estimado))
                Logger.warning("[Invalid DEATH: " + BitConverter.ToString(p.getBuffer()) + "]");
            Channel ch = ChannelsXML.getChannel(channelId);
            if (ch == null)
                return;
            Room room = ch.getRoom(roomId);
            if (room != null && room.round.Timer == null && room._state == RoomState.Battle)
            {
                SLOT killer = room.getSlot(killerId);
                if (killer != null && killer.state == SLOT_STATE.BATTLE)
                {
                    FragInfos info = new FragInfos
                    {
                        killerIdx = killerId,
                        killingType = 0, //1 - piercing | 2 - mass
                        weapon = weaponId,
                        x = killerX,
                        y = killerY,
                        z = killerZ,
                        flag = dieObjectId
                    };
                    bool isSuicide = false;
                    for (int i = 0; i < killsCount; i++)
                    {
                        byte weaponClass = p.readC();
                        byte deathInfo = p.readC();
                        float vicX = p.readT();
                        float vicY = p.readT();
                        float vicZ = p.readT();
                        int vicFlag = p.readC();
                        int victimId = deathInfo & 15;
                        SLOT victim = room.getSlot(victimId);
                        if (victim != null && victim.state == SLOT_STATE.BATTLE)
                        {
                            Frag frag = new Frag(deathInfo) { flag = (byte)vicFlag, victimWeaponClass = weaponClass, x = vicX, y = vicY, z = vicZ };
                            if (info.killerIdx == victimId)
                                isSuicide = true;
                            info.frags.Add(frag);
                        }
                    }
                    info.killsCount = (byte)info.frags.Count;
                    Game_SyncNet.genDeath(room, killer, info, isSuicide);
                }
            }
        }
        public static void RegistryFragInfos(Room room, SLOT killer, out int score, bool isBotMode, bool isSuicide, FragInfos kills)
        {
            score = 0;
            ITEM_CLASS weaponClass = (ITEM_CLASS)ComDiv.getIdStatics(kills.weapon, 1);
            for (int i = 0; i < kills.frags.Count; i++)
            {
                Frag frag = kills.frags[i];
                CharaDeath deathType = (CharaDeath)(frag.hitspotInfo >> 4);
                if (kills.killsCount - (isSuicide ? 1 : 0) > 1)
                    frag.killFlag |= deathType == CharaDeath.BOOM || deathType == CharaDeath.OBJECT_EXPLOSION || deathType == CharaDeath.POISON || deathType == CharaDeath.HOWL || deathType == CharaDeath.TRAMPLED || weaponClass == ITEM_CLASS.SHOTGUN ? KillingMessage.MassKill : KillingMessage.PiercingShot;
                else
                {
                    int num2 = 0;
                    if (deathType == CharaDeath.HEADSHOT)
                        num2 = 4;
                    else if (deathType == CharaDeath.DEFAULT && weaponClass == ITEM_CLASS.KNIFE)
                        num2 = 6;
                    if (num2 > 0)
                    {
                        int num3 = killer.lastKillState >> 12;
                        if (num2 == 4)
                        {
                            if (num3 != 4)
                                killer.repeatLastState = false;
                            killer.lastKillState = num2 << 12 | (killer.killsOnLife + 1);
                            if (killer.repeatLastState)
                                frag.killFlag |= (killer.lastKillState & 16383) <= 1 ? KillingMessage.Headshot : KillingMessage.ChainHeadshot;
                            else
                            {
                                frag.killFlag |= KillingMessage.Headshot;
                                killer.repeatLastState = true;
                            }
                        }
                        else if (num2 == 6)
                        {
                            if (num3 != 6)
                                killer.repeatLastState = false;
                            killer.lastKillState = num2 << 12 | (killer.killsOnLife + 1);
                            if (killer.repeatLastState && (killer.lastKillState & 16383) > 1)
                                frag.killFlag |= KillingMessage.ChainSlugger;
                            else
                                killer.repeatLastState = true;
                        }
                    }
                    else
                    {
                        killer.lastKillState = 0;
                        killer.repeatLastState = false;
                    }
                }
                int victimId = frag.VictimSlot;
                SLOT victimSlot = room._slots[victimId];
                if (victimSlot.killsOnLife > 3) frag.killFlag |= KillingMessage.ChainStopper;
                if ((kills.weapon == 19016 || kills.weapon == 19022) && kills.killerIdx == victimId || victimSlot.specGM)
                { }
                else
                    victimSlot.allDeaths++; //exprosao acid
                if (killer._team != victimSlot._team)
                {
                    score += AllUtils.getKillScore(frag.killFlag);
                    killer.allKills++;
                    if (killer._deathState == DeadEnum.isAlive)
                        killer.killsOnLife++;
                    if (victimSlot._team == 0)
                    {
                        room._redDeaths++;
                        room._blueKills++;
                    }
                    else
                    {
                        room._blueDeaths++;
                        room._redKills++;
                    }
                    if (room.room_type == 7)
                        if (killer._team == 0) room.red_dino += 4;
                        else room.blue_dino += 4;
                }
                victimSlot.lastKillState = 0;
                victimSlot.killsOnLife = 0;
                victimSlot.repeatLastState = false;
                victimSlot.passSequence = 0;
                victimSlot._deathState = DeadEnum.isDead;
                if (!isBotMode)
                    AllUtils.CompleteMission(room, victimSlot, MISSION_TYPE.DEATH, 0);
                if (deathType == CharaDeath.HEADSHOT)
                    killer.headshots++;
            }
        }
        public static void EndBattleByDeath(Room room, SLOT killer, bool isBotMode, bool isSuicide)
        {
            if ((room.room_type == 1 || room.room_type == 8 || room.room_type == 13) && !isBotMode)
                AllUtils.BattleEndKills(room, isBotMode);
            else if (!killer.specGM && (room.room_type == 2 || room.room_type == 4)) //Destruição e Supressão
            {
                int redDeaths, blueDeaths, allRed, allBlue, winner = 0;
                room.getPlayingPlayers(true, out allRed, out allBlue, out redDeaths, out blueDeaths);
                if (redDeaths == allRed && killer._team == 0 && isSuicide && !room.C4_actived)
                {
                    winner = 1;
                    room.blue_rounds++;
                    AllUtils.BattleEndRound(room, winner, true);
                }
                else if (blueDeaths == allBlue && killer._team == 1)
                {
                    room.red_rounds++;
                    AllUtils.BattleEndRound(room, winner, true);
                }
                else if (redDeaths == allRed && killer._team == 1)
                {
                    if (!room.C4_actived)
                    {
                        winner = 1;
                        room.blue_rounds++;
                    }
                    else if (isSuicide)
                        room.red_rounds++;
                    AllUtils.BattleEndRound(room, winner, false);
                }
                else if (blueDeaths == allBlue && killer._team == 0)
                {
                    if (!isSuicide || !room.C4_actived)
                        room.red_rounds++;
                    else
                    {
                        winner = 1;
                        room.blue_rounds++;
                    }
                    AllUtils.BattleEndRound(room, winner, true);
                }
            }
        }
    }
}
/*
 * Arquivo: VOTEKICK_START_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 08/06/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class VOTEKICK_START_REC : ReceiveGamePacket
    {
        private int motive, slotIdx;
        private uint erro;
        public VOTEKICK_START_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slotIdx = readC();
            motive = readC();
            //motive 0=NoManner|1=IllegalProgram|2=Abuse|3=ETC
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                Room room = p == null ? null : p._room;
                if (room == null || room._state != RoomState.Battle || p._slotId == slotIdx)
                    return;
                SLOT slot = room.getSlot(p._slotId);
                if (slot != null && slot.state == SLOT_STATE.BATTLE && room._slots[slotIdx].state == SLOT_STATE.BATTLE)
                {
                    int redPlayers, bluePlayers;
                    room.getPlayingPlayers(true, out redPlayers, out bluePlayers);
                    //if (redPlayers < 3 && bluePlayers == 1 ||
                    //bluePlayers < 3 && redPlayers == 1) erro = 0x800010E2;
                    if (p._rank < ConfigGS.minRankVote && !p.HaveGMLevel()) erro = 0x800010E4;
                    else if (room.vote.Timer != null) erro = 0x800010E0;
                    else if (slot.NextVoteDate > DateTime.Now) erro = 0x800010E1;
                    _client.SendPacket(new VOTEKICK_CHECK_PAK(erro));
                    if (erro > 0)
                        return;
                    slot.NextVoteDate = DateTime.Now.AddMinutes(1);
                    room.votekick = new VoteKick(slot._id, slotIdx)
                    {
                        motive = motive
                    };
                    ChargeVoteKickArray(room);
                    using (VOTEKICK_START_PAK packet = new VOTEKICK_START_PAK(room.votekick))
                        room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0, p._slotId, slotIdx);
                    AllUtils.LogVotekickStart(room, p, slot);
                    room.StartVote();
                }
            }
            catch (Exception ex)
            {
                Logger.info("VOTEKICK_START_REC: " + ex.ToString());
            }
        }
        /// <summary>
        /// Configura a array com os jogadores em partida.
        /// </summary>
        /// <param name="room"></param>
        private void ChargeVoteKickArray(Room room)
        {
            for (int i = 0; i < 16; i++)
            {
                SLOT slot = room._slots[i];
                room.votekick.TotalArray[i] = (slot.state == SLOT_STATE.BATTLE);
            }
        }
    }
}
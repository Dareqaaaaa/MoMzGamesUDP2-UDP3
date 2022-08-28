using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class VOTEKICK_UPDATE_REC : ReceiveGamePacket
    {
        private byte type;
        public VOTEKICK_UPDATE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            type = readC();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                Room r = p == null ? null : p._room;
                SLOT slot;
                if (r == null || r._state != RoomState.Battle || r.vote.Timer == null ||
                    r.votekick == null || !r.getSlot(p._slotId, out slot) || slot.state != SLOT_STATE.BATTLE)
                    return;
                VoteKick vote = r.votekick;
                if (vote._votes.Contains(p._slotId))
                {
                    _client.SendPacket(new VOTEKICK_UPDATE_RESULT_PAK(0x800010F1));
                    return;
                }
                lock (vote._votes)
                {
                    vote._votes.Add(slot._id);
                }
                if (type == 0)
                {
                    vote.kikar++;
                    if (slot._team == vote.victimIdx % 2)
                        vote.allies++;
                    else
                        vote.enemys++;
                }
                else vote.deixar++;
                if (vote._votes.Count >= vote.GetInGamePlayers())
                {
                    r.vote.Timer = null;
                    AllUtils.votekickResult(r);
                }
                else
                {
                    using (VOTEKICK_UPDATE_PAK packet = new VOTEKICK_UPDATE_PAK(vote))
                        r.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                }
            }
            catch (Exception ex)
            {
                Logger.info("VOTEKICK_UPDATE_REC: " + ex.ToString());
            }
        }
    }
}
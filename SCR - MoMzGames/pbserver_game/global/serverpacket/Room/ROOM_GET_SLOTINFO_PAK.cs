using Core;
using Core.models.account.clan;
using Core.models.room;
using Core.server;
using Game.data.managers;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
    public class ROOM_GET_SLOTINFO_PAK : SendPacket
    {
        private Room room;
        public ROOM_GET_SLOTINFO_PAK(Room r)
        {
            room = r;
        }

        public override void write()
        {
            try
            {
                if (room == null)
                    return;
                writeH(3861);
                if (room.getLeader() == null)
                    room.setNewLeader(-1, 0, room._leader, false);
                if (room.getLeader() != null)
                {
                    writeD(room._leader);
                    for (int i = 0; i < 16; i++)
                    {
                        SLOT slot = room._slots[i];
                        Account pR = room.getPlayerBySlot(slot);
                        if (pR != null)
                        {
                            Clan clan = ClanManager.getClan(pR.clanId);
                            writeC((byte)slot.state);
                            writeC((byte)pR.getRank());
                            writeD(clan._id);
                            writeD(pR.clanAccess);
                            writeC((byte)clan._rank);
                            writeD(clan._logo);
                            writeC((byte)pR.pc_cafe);
                            writeC((byte)pR.tourneyLevel);
                            writeD((uint)pR.effects);
                            //writeC((byte)pR.effect_1); //Lista de cupons 1 [1 - 90% Colete do BOPE Reforçado || 2 - Ketupat || 4 - 20% Colete Reforçado || 8 - Hollow Point Ammo Plus || 16 - 10% Colete Plus || 32 - 5% HP || 64 - Hollowpoint F. || 128 - Explosivo extra]
                            //writeC((byte)pR.effect_2); //Lista de cupons 2 [1 - C4 Speed || 2 - Hollowpoint || 4 - Bala de Ferro || 8 - 5% Colete || 16 - +1s piscando || 32 - +10% HP || 64 - Recarregamento rápido || 128 - Troca rápida] / [1/2/4/8/16/32/64/128]
                            //writeC((byte)pR.effect_3); //Lista de cupons 3 [1 - Flash Bang Protection || 2 - Receber drop || 4 - +40% de munição || 16 - 30% Respawn || 32 - +50% Respawn || 64 - +100% Respawn || 128 - +10% de munição]  [1/2/4/8/16/32/64/128]
                            //writeC((byte)pR.effect_4); //Lista de cupons 4 [1 - Item especial extra || 4 - Bala de ferro]
                            //writeC((byte)pR.effect_5); //Lista de cupons 5 [2 - Receber drop] - DEAD?
                            writeS(clan._name, 17);
                            writeD(0);
                            writeC(31);
                        }
                        else
                        {
                            writeC((byte)slot.state);
                            writeB(new byte[10]);
                            writeD(4294967295);
                            writeB(new byte[28]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[ROOM_GET_SLOTINFO_PAK] " + ex.ToString());
            }
        }
    }
}
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class ROOM_GET_SLOTONEINFO_PAK : SendPacket
    {
        private Account p;
        private Clan clan;
        public ROOM_GET_SLOTONEINFO_PAK(Account player)
        {
            p = player;
            if (p != null)
                clan = ClanManager.getClan(p.clanId);
        }
        public ROOM_GET_SLOTONEINFO_PAK(Account player, Clan c)
        {
            p = player;
            clan = c;
        }
        public override void write()
        {
            if (p._room == null || p._slotId == -1)
                return;
            writeH(3909);
            writeD(p._slotId);
            writeC((byte)p._room._slots[p._slotId].state);
            writeC((byte)p.getRank());
            writeD(clan._id);
            writeD(p.clanAccess);
            writeC((byte)clan._rank);
            writeD(clan._logo);
            writeC((byte)p.pc_cafe);
            writeC((byte)p.tourneyLevel);
            writeD((uint)p.effects);
            writeS(clan._name, 17);
            writeD(0);
            writeC(31);
            writeS(p.player_name, 33);
            writeC((byte)p.name_color);
        }
    }
}
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BASE_2612_PAK : SendPacket
    {
        private Account p;
        private Clan clan;
        public BASE_2612_PAK(Account player)
        {
            p = player;
            clan = ClanManager.getClan(p.clanId);
        }

        public override void write()
        {
            writeH(2612);
            writeS(p.player_name, 33);
            writeD(p._exp);
            writeD(p._rank);
            writeD(p._rank);
            writeD(p._gp);
            writeD(p._money);
            writeD(clan._id);
            writeD(p.clanAccess);
            writeQ(0);
            writeC((byte)p.pc_cafe);
            writeC((byte)p.tourneyLevel);
            writeC((byte)p.name_color);
            writeS(clan._name, 17);
            writeC((byte)clan._rank);
            writeC((byte)clan.getClanUnit());
            writeD(clan._logo);
            writeC((byte)clan._name_color);
            writeD(10000);
            writeC(0);
            writeD(0);
            writeD(p.LastRankUpDate); //109 BYTES
        }
    }
}
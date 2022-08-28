using Core.server;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
    public class BASE_2626_PAK : SendPacket
    {
        private Account p;
        public BASE_2626_PAK(Account player)
        {
            p = player;
        }

        public override void write()
        {
            writeH(2626);
            writeB(BitConverter.GetBytes(p.player_id), 0, 4);
            writeQ(p._titles.Flags);
            writeC((byte)p._titles.Equiped1);
            writeC((byte)p._titles.Equiped2);
            writeC((byte)p._titles.Equiped3);
            writeD(p._titles.Slots);
        }
    }
}
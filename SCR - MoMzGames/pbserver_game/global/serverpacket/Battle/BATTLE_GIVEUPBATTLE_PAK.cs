using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_GIVEUPBATTLE_PAK : SendPacket
    {
        private Room _r;
        private int _oldLeader;
        public BATTLE_GIVEUPBATTLE_PAK(Room room, int oldLeader)
        {
            _r = room;
            _oldLeader = oldLeader;
        }

        public override void write()
        {
            writeH(3347);
            writeD(_r._leader);
            for (int i = 0; i < 16; i++)
            {
                if (_oldLeader == i)
                    writeB(new byte[13]);
                else
                {
                    Account p = _r.getPlayerBySlot(i);
                    if (p != null)
                    {
                        writeIP(p.PublicIP);
                        writeH(29890);
                        writeB(p.LocalIP);
                        writeH(29890);
                        writeC(0);
                    }
                    else writeB(new byte[13]);
                }
            }
        }
    }
}
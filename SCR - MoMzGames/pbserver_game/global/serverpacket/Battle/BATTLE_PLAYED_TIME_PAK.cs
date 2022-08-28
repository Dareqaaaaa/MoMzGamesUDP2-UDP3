using Core.managers.events;
using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_PLAYED_TIME_PAK : SendPacket
    {
        private int _type;
        private PlayTimeModel ev;
        public BATTLE_PLAYED_TIME_PAK(int type, PlayTimeModel eventPt)
        {
            _type = type;
            ev = eventPt;
        }

        public override void write()
        {
            writeH(3911);
            writeC((byte)_type);
            writeS(ev._title, 30);
            writeD(ev._goodReward1);
            writeD(ev._goodReward2);
            writeQ(ev._time);
        }
    }
}
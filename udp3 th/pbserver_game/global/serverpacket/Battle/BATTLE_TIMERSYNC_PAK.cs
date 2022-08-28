using Core.server;
using Game.data.model;
using Game.data.utils;

namespace Game.global.serverpacket
{
    public class BATTLE_TIMERSYNC_PAK : SendPacket
    {
        private Room _r;
        public BATTLE_TIMERSYNC_PAK(Room r)
        {
            _r = r;
        }

        public override void write()
        {
            writeH(3371);
            writeC((byte)_r.rodada);
            writeD(_r.getInBattleTimeLeft());
            writeH(AllUtils.getSlotsFlag(_r, true, false));
        }
    }
}
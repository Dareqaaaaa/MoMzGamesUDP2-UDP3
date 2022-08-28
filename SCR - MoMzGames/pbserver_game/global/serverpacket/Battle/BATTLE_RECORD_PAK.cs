using Core.models.room;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_RECORD_PAK : SendPacket
    {
        private Room _r;
        public BATTLE_RECORD_PAK(Room r)
        {
            _r = r;
        }

        public override void write()
        {
            writeH(3363);
            writeH((ushort)_r._redKills);
            writeH((ushort)_r._redDeaths);
            writeH((ushort)_r._blueKills);
            writeH((ushort)_r._blueDeaths);
            for (int i = 0; i < 16; i++)
            {
                SLOT slot = _r._slots[i];
                writeH((ushort)slot.allKills);
                writeH((ushort)slot.allDeaths);
            }
        }
    }
}
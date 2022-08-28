using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_DINO_PLACAR_PAK : SendPacket
    {
        private Room _r;
        public BATTLE_DINO_PLACAR_PAK(Room r)
        {
            _r = r;
        }

        public override void write()
        {
            writeH(3389);
            writeH((ushort)_r.red_dino);
            writeH((ushort)_r.blue_dino);
        }
    }
}
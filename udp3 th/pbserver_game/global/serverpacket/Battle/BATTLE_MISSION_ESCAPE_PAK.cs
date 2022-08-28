using Core.models.room;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_MISSION_ESCAPE_PAK : SendPacket
    {
        private Room r;
        private SLOT slot;
        public BATTLE_MISSION_ESCAPE_PAK(Room room, SLOT slot)
        {
            r = room;
            this.slot = slot;
        }

        public override void write()
        {
            writeH(3383);
            writeH((ushort)r.red_dino);
            writeH((ushort)r.blue_dino);
            writeD(slot._id); //slot do jogador que passou no portal
            writeH((short)slot.passSequence); //vezes seguidas que passo no portal
            /*
             * 
             *[LogP] recebido 'tam.': 10
              [LogP] recebido 'pacote': 3383 --Modo escolta
              00000000   0A 00 37 0D 00 00 00 00  00 00 00 00 01 00         ··7···········  
             */
        }
    }
}
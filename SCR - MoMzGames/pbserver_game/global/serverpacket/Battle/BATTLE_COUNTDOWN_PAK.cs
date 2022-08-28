using Core.models.enums;
using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_COUNTDOWN_PAK : SendPacket
    {
        private CountDownEnum type;
        public BATTLE_COUNTDOWN_PAK(CountDownEnum timer)
        {
            type = timer;
        }
        public override void write()
        {
            writeH(3340);
            writeC((byte)type);
        }
    }
}
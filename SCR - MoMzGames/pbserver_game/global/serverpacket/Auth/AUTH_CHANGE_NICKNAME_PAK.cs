using Core.server;

namespace Game.global.serverpacket
{
    public class AUTH_CHANGE_NICKNAME_PAK : SendPacket
    {
        private string name;
        public AUTH_CHANGE_NICKNAME_PAK(string name)
        {
            this.name = name;
        }

        public override void write()
        {
            writeH(300);
            writeC((byte)name.Length);
            writeS(name, name.Length);
        }
    }
}
using Core.server;

namespace Game.global.serverpacket
{
    public class ROOM_CHANGE_PASSWD_PAK : SendPacket
    {
        private string _pass;
        public ROOM_CHANGE_PASSWD_PAK(string pass)
        {
            _pass = pass;
        }

        public override void write()
        {
            writeH(3907);
            writeS(_pass, 4);
        }
    }
}
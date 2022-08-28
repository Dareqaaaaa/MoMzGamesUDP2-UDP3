using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CHANGE_NAME_PAK : SendPacket
    {
        private string _name;
        public CLAN_CHANGE_NAME_PAK(string name)
        {
            _name = name;
        }

        public override void write()
        {
            writeH(1368);
            writeS(_name, 17);
        }
    }
}
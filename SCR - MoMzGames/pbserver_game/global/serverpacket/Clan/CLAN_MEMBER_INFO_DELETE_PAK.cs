using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_MEMBER_INFO_DELETE_PAK : SendPacket
    {
        private long _pId;
        public CLAN_MEMBER_INFO_DELETE_PAK(long pId)
        {
            _pId = pId;
        }

        public override void write()
        {
            writeH(1353);
            writeQ(_pId);
        }
    }
}
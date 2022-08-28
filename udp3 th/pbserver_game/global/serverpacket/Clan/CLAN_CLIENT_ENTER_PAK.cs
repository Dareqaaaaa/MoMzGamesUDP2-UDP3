using Core.server;
using Game.data.managers;
using System;

namespace Game.global.serverpacket
{
    public class CLAN_CLIENT_ENTER_PAK : SendPacket
    {
        private int _type, _clanId;
        public CLAN_CLIENT_ENTER_PAK(int id, int access)
        {
            _clanId = id;
            _type = access;
        }

        public override void write()
        {
            writeH(1442);
            writeD(_clanId);
            writeD(_type);
            if (_clanId == 0 || _type == 0)
            {
                writeD(ClanManager._clans.Count);
                writeC(170);
                writeH((ushort)Math.Ceiling(ClanManager._clans.Count / 170d));
                writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
            }
        }
    }
}
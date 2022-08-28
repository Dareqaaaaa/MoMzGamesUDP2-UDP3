/*
 * Arquivo: CLAN_CLIENT_CLAN_LIST_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 27/07/2017
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CLIENT_CLAN_LIST_PAK : SendPacket
    {
        private uint _page;
        private int _count;
        private byte[] _array;
        public CLAN_CLIENT_CLAN_LIST_PAK(uint page, int count, byte[] array)
        {
            _page = page;
            _count = count;
            _array = array;
        }
        public override void write()
        {
            writeH(1446);
            writeD(_page);
            writeC((byte)_count);
            writeB(_array);
        }
    }
}
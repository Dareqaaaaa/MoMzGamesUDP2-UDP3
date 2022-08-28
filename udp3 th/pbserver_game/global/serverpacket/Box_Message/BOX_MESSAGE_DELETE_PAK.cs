using Core.server;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class BOX_MESSAGE_DELETE_PAK : SendPacket
    {
        private uint _erro;
        private List<object> _objs;
        public BOX_MESSAGE_DELETE_PAK(uint erro, List<object> objs)
        {
            _erro = erro;
            _objs = objs;
        }

        public override void write()
        {
            writeH(425);
            writeD(_erro);
            writeC((byte)_objs.Count);
            foreach (int obj in _objs)
                writeD(obj);
        }
    }
}
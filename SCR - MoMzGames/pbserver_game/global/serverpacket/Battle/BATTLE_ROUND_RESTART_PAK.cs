using Core.server;
using Game.data.model;
using Game.data.utils;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class BATTLE_ROUND_RESTART_PAK : SendPacket
    {
        private Room _r;
        private List<int> _dinos;
        private bool isBotMode;
        public BATTLE_ROUND_RESTART_PAK(Room r, List<int> dinos, bool isBotMode)
        {
            _r = r;
            _dinos = dinos;
            this.isBotMode = isBotMode;
        }
        public BATTLE_ROUND_RESTART_PAK(Room r)
        {
            _r = r;
            _dinos = AllUtils.getDinossaurs(r, false, -1);
            isBotMode = _r.isBotMode();
        }
        public override void write()
        {
            writeH(3351);
            writeH(AllUtils.getSlotsFlag(_r, false, false));
            if (_r.room_type == 8)
                writeB(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            else if (isBotMode)
                writeB(new byte[10]);
            else if (_r.room_type == 7 || _r.room_type == 12)
            {
                int TRex = _dinos.Count == 1 || _r.room_type == 12 ? 255 : _r.TRex;
                writeC((byte)TRex); //T-Rex || 255 (não tem t-rex)
                foreach (int slot in _dinos)
                    if (slot != _r.TRex && _r.room_type == 7 || _r.room_type == 12)
                        writeC((byte)slot);
                int falta = 8 - _dinos.Count - (TRex == 255 ? 1 : 0);
                for (int i = 0; i < falta; i++)
                    writeC(255);
                writeC(255);
                writeC(255);
            }
            else
                writeB(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });//writeB(new byte[] { 255, 1, 255, 255, 255, 255, 255, 255, 255, 255 });
        }
    }
}
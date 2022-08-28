using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class CLAN_MEMBER_CONTEXT_PAK : SendPacket
    {
        private int erro, playersCount;
        public CLAN_MEMBER_CONTEXT_PAK(int erro, int playersCount)
        {
            this.erro = erro;
            this.playersCount = playersCount;
        }
        public CLAN_MEMBER_CONTEXT_PAK(int erro)
        {
            this.erro = erro;
        }
        public override void write()
        {
            writeH(1307);
            writeD(erro);
            if (erro == 0)
            {
                writeC((byte)playersCount);
                writeC(14);
                writeC((byte)Math.Ceiling(playersCount / 14d));
                writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
            }
        }
    }
}
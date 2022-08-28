using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_MEMBER_LIST_PAK : SendPacket
    {
        private byte[] array;
        private int erro, page, count;
        public CLAN_MEMBER_LIST_PAK(int page, int count, byte[] array)
        {
            this.page = page;
            this.count = count;
            this.array = array;
        }
        public CLAN_MEMBER_LIST_PAK(int erro)
        {
            this.erro = erro;
        }
        public override void write()
        {
            writeH(1309);
            writeD(erro);
            if (erro < 0)
                return;
            writeC((byte)page);
            writeC((byte)count);
            writeB(array);
        }
    }
}
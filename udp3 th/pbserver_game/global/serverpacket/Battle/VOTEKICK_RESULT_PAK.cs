using Core.models.room;
using Core.server;

namespace Game.global.serverpacket
{
    public class VOTEKICK_RESULT_PAK : SendPacket
    {
        private VoteKick vote;
        private uint erro;
        public VOTEKICK_RESULT_PAK(uint erro, VoteKick vote)
        {
            this.erro = erro;
            this.vote = vote;
        }

        public override void write()
        {
            writeH(3403);
            writeC((byte)vote.victimIdx);
            writeC((byte)vote.kikar);
            writeC((byte)vote.deixar);
            writeD(erro);
            //[2147488000] - cancelou a votação
            //[2147488001] - Sem votos aliados
            //[2147488002] - Sem votos adversários
            //[2147488003] - Patente não pode abrir
            //
        }
    }
}
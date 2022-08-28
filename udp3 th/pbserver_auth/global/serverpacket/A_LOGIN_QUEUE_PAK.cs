/*
 * Arquivo: A_LOGIN_QUEUE_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class A_LOGIN_QUEUE_PAK : SendPacket
    {
        private int queue_pos, estimated_time;
        public A_LOGIN_QUEUE_PAK(int queue_pos, int estimated_time)
        {
            this.queue_pos = queue_pos;
            this.estimated_time = estimated_time;
        }

        public override void write()
        {
            writeH(2676);
            writeD(queue_pos); //Posição na fila
            writeD(estimated_time); //Tempo estimado para entrar (Segundos)
        }
    }
}
using Core.server;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
    public class BASE_CHAT_ERROR_PAK : SendPacket
    {
        private int erro, banTime;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="erro">0=Nenhum bloqueio; 1=?; 2="Primeiro aviso"</param>
        /// <param name="time">Duração do bloqueio</param>
        public BASE_CHAT_ERROR_PAK(int erro, int time = 0)
        {
            this.erro = erro;
            banTime = time;
        }
        public override void write()
        {
            writeH(2628);
            writeC((byte)erro); //Result/Type (0 = Não bloqueado | 2 = Primeiro block | 1 = ?)
            if (erro > 0)
                writeD(banTime); //Segundos
            /*
             * 1=STR_MESSAGE_BLOCK_ING
             * 2=STR_MESSAGE_BLOCK_START
             */
        }
    }
}
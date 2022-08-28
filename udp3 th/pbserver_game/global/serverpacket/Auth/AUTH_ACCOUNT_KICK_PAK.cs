/*
 * Arquivo: AUTH_ACCOUNT_KICK_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 17/12/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Game.global.serverpacket
{
    public class AUTH_ACCOUNT_KICK_PAK : SendPacket
    {
        private int _type;
        public AUTH_ACCOUNT_KICK_PAK(int type)
        {
            _type = type;
        }

        public override void write()
        {
            writeH(513);
            writeC((byte)_type); //0 (O jogo será finalizado por solicitação do servidor) || 1 (Conexão simultanêa) || 2 (Jogo finalizado em instantes [GM]) || 3
        }
    }
}
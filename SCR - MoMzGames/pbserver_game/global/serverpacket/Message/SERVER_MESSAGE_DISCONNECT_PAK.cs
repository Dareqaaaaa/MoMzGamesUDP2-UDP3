/*
 * Arquivo: SERVER_MESSAGE_DISCONNECT_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class SERVER_MESSAGE_DISCONNECT_PAK : SendPacket
    {
        private uint _erro;
        private bool type;
        public SERVER_MESSAGE_DISCONNECT_PAK(uint erro, bool HackUse)
        {
            _erro = erro;
            type = HackUse;
        }

        public override void write()
        {
            writeH(2062);
            writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
            writeD(_erro);
            writeD(type); //Se for igual a 1, novo writeD (Da DC no cliente, Programa ilegal)
            if (type)
                writeD(0);
        }
    }
}
/*
 * Arquivo: A_2678_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 21/10/2016
 * Sintam inveja, não nos atinge
 */

using Auth.global.serverpacket;
using Core;
using System;

namespace Auth.global.clientpacket
{
    public class A_2678_REC : ReceiveLoginPacket
    {
        public A_2678_REC(LoginClient lc, byte[] buff)
        {
            makeme(lc, buff);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                _client.SendPacket(new A_2678_PAK());
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
    }
}
/*
 * Arquivo: BASE_USER_EXIT_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 21/10/2016
 * Sintam inveja, não nos atinge
 */

using Auth.global.serverpacket;
using Core;
using System;

namespace Auth.global.clientpacket
{
    public class BASE_USER_EXIT_REC : ReceiveLoginPacket
    {
        public BASE_USER_EXIT_REC(LoginClient lc, byte[] buff)
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
                _client.SendPacket(new BASE_USER_EXIT_PAK());
                _client.Close(5000, true);
            }
            catch (Exception ex)
            {
                Logger.warning("[BASE_USER_EXIT_REC] " + ex.ToString());
            }
        }
    }
}
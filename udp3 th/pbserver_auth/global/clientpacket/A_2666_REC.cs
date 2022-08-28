/*
 * Arquivo: A_2666_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 21/10/2016
 * Sintam inveja, não nos atinge
 */

using Auth.global.serverpacket;
using Core;
using System;

namespace Auth.global.clientpacket
{
    public class A_2666_REC : ReceiveLoginPacket
    {
        public A_2666_REC(LoginClient lc, byte[] buff)
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
                _client.SendPacket(new BASE_RANK_AWARDS_PAK());
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
    }
}
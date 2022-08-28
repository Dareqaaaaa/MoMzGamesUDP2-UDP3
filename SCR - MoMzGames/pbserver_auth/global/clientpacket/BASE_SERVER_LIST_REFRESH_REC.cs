/*
 * Arquivo: BASE_SERVER_LIST_REFRESH_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 21/10/2016
 * Sintam inveja, não nos atinge
 */

using Auth.global.serverpacket;
using Core;
using System;

namespace Auth.global.clientpacket
{
    public class BASE_SERVER_LIST_REFRESH_REC : ReceiveLoginPacket
    {
        public BASE_SERVER_LIST_REFRESH_REC(LoginClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                if (_client != null)
                    _client.SendPacket(new BASE_SERVER_LIST_REFRESH_PAK());
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
    }
}
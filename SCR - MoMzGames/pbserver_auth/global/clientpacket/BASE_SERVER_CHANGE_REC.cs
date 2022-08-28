/*
 * Arquivo: BASE_SERVER_CHANGE_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 17/07/2016
 * Sintam inveja, não nos atinge
 */

using Auth.global.serverpacket;
using Core;
using System;

namespace Auth.global.clientpacket
{
    public class BASE_SERVER_CHANGE_REC : ReceiveLoginPacket
    {
        public BASE_SERVER_CHANGE_REC(LoginClient client, byte[] data)
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
                if (_client == null || _client._player == null)
                    return;
                _client.SendPacket(new BASE_SERVER_CHANGE_PAK(0));
                _client.Close(0, false);
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
    }
}
/*
 * Arquivo: BASE_USER_INVENTORY_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 17/12/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.model;
using Auth.global.serverpacket;
using Core;
using System;

namespace Auth.global.clientpacket
{
    public class BASE_USER_INVENTORY_REC : ReceiveLoginPacket
    {
        public BASE_USER_INVENTORY_REC(LoginClient client, byte[] data)
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
                Account p = _client._player;
                if (p == null)
                    return;
                _client.SendPacket(new BASE_USER_INVENTORY_PAK(p._inventory._items));
            }
            catch (Exception ex)
            {
                Logger.warning("[BASE_INVENTORY_REC] " + ex.ToString());
            }
        }
    }
}
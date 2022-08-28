/*
 * Arquivo: AUTH_FIND_USER_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 14/06/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class AUTH_FIND_USER_REC : ReceiveGamePacket
    {
        private string name;
        public AUTH_FIND_USER_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            name = readS(33);
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || p.player_name.Length == 0 || p.player_name == name)
                    return;
                Account user = AccountManager.getAccount(name, 1, 0);
                _client.SendPacket(new AUTH_FIND_USER_PAK(user == null ? 2147489795 : !user._isOnline ? 2147489796 : 0, user));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
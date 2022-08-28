/*
 * Arquivo: BASE_USER_CONFIGS_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 23/09/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.model;
using Auth.global.serverpacket;
using Core;
using Core.managers;
using Core.models.account;
using System;
using System.Collections.Generic;

namespace Auth.global.clientpacket
{
    public class BASE_USER_CONFIGS_REC : ReceiveLoginPacket
    {
        public BASE_USER_CONFIGS_REC(LoginClient client, byte[] data)
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
                if (p == null || p._myConfigsLoaded)
                    return;
                string ExitURL = LoginManager.Config.ExitURL;
                _client.SendPacket(new BASE_EXIT_URL_PAK(ExitURL));
                if (p.FriendSystem._friends.Count > 0)
                    _client.SendPacket(new BASE_USER_FRIENDS_PAK(p.FriendSystem._friends));
                SendMessagesList(p);
                _client.SendPacket(new BASE_USER_CONFIG_PAK(0, p._config));
            }
            catch (Exception ex)
            {
                Logger.warning("[BASE_USER_CONFIGS_REC] " + ex.ToString());
            }
        }
        private void SendMessagesList(Account p)
        {
            List<Message> msgs = MessageManager.getMessages(p.player_id);
            if (msgs.Count == 0)
                return;
            MessageManager.RecicleMessages(p.player_id, msgs);
            if (msgs.Count == 0)
                return;

            int pages = (int)Math.Ceiling(msgs.Count / 25d);
            for (int i = 0; i < pages; i++)
                _client.SendPacket(new BASE_USER_MESSAGES_PAK(i, msgs));
        }
    }
}
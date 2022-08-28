/*
 * Arquivo: BASE_USER_INFO_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 16/03/2017
 * Sintam inveja, não nos atinge
 */

using Auth.global.serverpacket;
using Core;
using Core.managers;
using System;

namespace Auth.global.clientpacket
{
    public class BASE_USER_INFO_REC : ReceiveLoginPacket
    {
        public BASE_USER_INFO_REC(LoginClient client, byte[] data)
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
                _client.SendPacket(new BASE_USER_INFO_PAK(_client._player));
                //_client.SendPacket(new SERVER_MESSAGE_EVENT_RANKUP_PAK());
                //_client.SendPacket(new SERVER_MESSAGE_EVENT_QUEST_PAK());
            }
            catch (Exception ex)
            {
                Logger.warning("[BASE_USER_INFO_REC] " + ex.ToString());
            }
        }
    }
}
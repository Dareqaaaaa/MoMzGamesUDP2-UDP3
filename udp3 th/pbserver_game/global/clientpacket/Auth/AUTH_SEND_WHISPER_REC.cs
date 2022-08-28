/*
 * Arquivo: AUTH_SEND_WHISPER_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 28/05/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class AUTH_SEND_WHISPER_REC : ReceiveGamePacket
    {
        private string receiverName, text;
        public AUTH_SEND_WHISPER_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            receiverName = readS(33);
            text = readS(readH());
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || p.player_name == receiverName)
                    return;
                Account pW = AccountManager.getAccount(receiverName, 1, 0);
                if (pW == null || !pW._isOnline)
                    _client.SendPacket(new AUTH_SEND_WHISPER_PAK(receiverName, text, 0x80000000));
                else
                    pW.SendPacket(new AUTH_RECV_WHISPER_PAK(p.player_name, text, p.UseChatGM()), false);
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
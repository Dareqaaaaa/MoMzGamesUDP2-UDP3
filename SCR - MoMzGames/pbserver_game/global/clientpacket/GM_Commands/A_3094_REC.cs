/*
 * Arquivo: A_3094_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 07/10/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Game.data.managers;
using Game.data.model;

namespace Game.global.clientpacket
{
    public class A_3094_REC : ReceiveGamePacket
    {
        private uint sessionId;
        private string name;
        public A_3094_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            sessionId = readUD();
            name = readS(readC());
        }

        public override void run()
        {
            if (_client == null || _client._player == null)
                return;
            Account p = _client._player;
            Channel ch = p.getChannel();
            if (ch == null || p._room != null || sessionId == uint.MaxValue)
                return;
            try
            {
                PlayerSession pS = ch.getPlayer(sessionId);
                if (pS == null)
                    return;
                Account pC = AccountManager.getAccount(pS._playerId, true);
                if (pC == null)
                    return;
                //Ativa quando usa "/EXIT (APELIDO)"
                Logger.warning("[3094] SessionId: " + sessionId + "; Name: " + name);
                //_client.SendPacket(new A_3094_PAK());
            }
            catch
            {
            }
        }
    }
}
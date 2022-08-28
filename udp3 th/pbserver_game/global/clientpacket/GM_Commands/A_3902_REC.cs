/*
 * Arquivo: A_3890_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 07/10/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class A_3902_REC : ReceiveGamePacket
    {
        public A_3902_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            if (_client == null || _client._player == null)
                return;
            try
            {
                //Ativa quando usa "/ROOMDEST"
                Logger.warning("[3902]");
            }
            catch
            {
            }
        }
    }
}
using Core;
using Core.managers;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class BOX_MESSAGE_DELETE_REC : ReceiveGamePacket
    {
        private uint erro;
        private List<object> objs = new List<object>();
        public BOX_MESSAGE_DELETE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            int count = readC();
            for (int i = 0; i < count; i++)
            {
                objs.Add(readD());
            }
        }

        public override void run()
        {
            if (_client._player == null)
                return;
            try
            {
                if (!MessageManager.DeleteMessages(objs, _client.player_id))
                    erro = 0x80000000;
                _client.SendPacket(new BOX_MESSAGE_DELETE_PAK(erro, objs));
                objs = null;
            }
            catch (Exception ex)
            {
                Logger.info("[BOX_MESSAGE_DELETE_REC] " + ex.ToString());
            }
        }
    }
}
using Core;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CM_2584 : ReceiveGamePacket
    {
        public byte[] unk;
        public CM_2584(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            unk = readB(59);
        }

        public override void run()
        {
            try
            {
                _client.SendPacket(new BASE_HACK_PAK(unk));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}

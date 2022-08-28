using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class BASE_SERVER_PASSW_REC : ReceiveGamePacket
    {
        private string pass;
        public BASE_SERVER_PASSW_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            pass = readS(readC());
        }

        public override void run()
        {
            if (_client != null)
                _client.SendPacket(new BASE_SERVER_PASSW_PAK(pass != ConfigGS.passw ? 0x80000000 : 0));
        }
    }
}
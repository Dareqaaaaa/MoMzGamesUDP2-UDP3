namespace Game.global.clientpacket
{
    public class BASE_HEARTBEAT_REC : ReceiveGamePacket
    {
        public BASE_HEARTBEAT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
        }
    }
}
namespace Game.global.clientpacket
{
    public class BASE_MISSION_SUCCESS_REC : ReceiveGamePacket
    {
        public BASE_MISSION_SUCCESS_REC(GameClient client, byte[] data)
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
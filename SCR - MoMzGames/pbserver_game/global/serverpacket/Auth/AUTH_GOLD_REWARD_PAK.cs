using Core.server;

namespace Game.global.serverpacket
{
    public class AUTH_GOLD_REWARD_PAK : SendPacket
    {
        private int gp, _gpIncrease, type;
        public AUTH_GOLD_REWARD_PAK(int increase, int gold, int type)
        {
            _gpIncrease = increase;
            gp = gold;
            this.type = type;
        }

        public override void write()
        {
            writeH(561);
            writeD(_gpIncrease);
            writeD(gp);
            writeD(type); //Faz aparecer STR_POPUP_GET_POINT
        }
    }
}
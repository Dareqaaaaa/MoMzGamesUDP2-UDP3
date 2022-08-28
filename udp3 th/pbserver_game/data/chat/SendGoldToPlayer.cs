using Core;
using Core.managers;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class SendGoldToPlayer
    {
        public static string SendByNick(string str)
        {
            Account pR = AccountManager.getAccount(str.Substring(3), 1, 0);
            return BaseGiveGold(pR);
        }
        public static string SendById(string str)
        {
            Account pR = AccountManager.getAccount(long.Parse(str.Substring(4)), 0);
            return BaseGiveGold(pR);
        }
        private static string BaseGiveGold(Account pR)
        {
            if (pR == null)
                return Translation.GetLabel("GiveGoldFail");
            if (PlayerManager.updateAccountGold(pR.player_id, pR._gp + 10000))
            {
                pR._gp += 10000;
                pR.SendPacket(new AUTH_WEB_CASH_PAK(0, pR._gp, pR._money), false);
                SEND_ITEM_INFO.LoadGoldCash(pR);
                return Translation.GetLabel("GiveGoldSuccess", pR.player_name);
            }
            else
                return Translation.GetLabel("GiveGoldFail2");
        }
    }
}
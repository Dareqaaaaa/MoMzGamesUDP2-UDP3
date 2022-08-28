using Core;
using Core.managers;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class SendCashToPlayer
    {
        public static string SendByNick(string str)
        {
            Account pR = AccountManager.getAccount(str.Substring(3), 1, 0);
            return BaseGiveCash(pR);
        }
        public static string SendById(string str)
        {
            Account pR = AccountManager.getAccount(long.Parse(str.Substring(4)), 0);
            return BaseGiveCash(pR);
        }
        private static string BaseGiveCash(Account pR)
        {
            if (pR == null)
                return Translation.GetLabel("GiveCashFail");
            if (PlayerManager.updateAccountCash(pR.player_id, pR._money + 3000))
            {
                pR._money += 3000;
                pR.SendPacket(new AUTH_WEB_CASH_PAK(0, pR._gp, pR._money), false);
                SEND_ITEM_INFO.LoadGoldCash(pR);
                return Translation.GetLabel("GiveCashSuccess", pR.player_name);
            }
            else
                return Translation.GetLabel("GiveCashFail2");
        }
    }
}
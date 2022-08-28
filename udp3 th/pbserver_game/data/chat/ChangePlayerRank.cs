using Core;
using Core.models.account.rank;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;

namespace Game.data.chat
{
    public static class ChangePlayerRank
    {
        public static string SetPlayerRank(string str)
        {
            string text = str.Substring(str.IndexOf(" ") + 1);
            string[] split = text.Split(' ');
            long player_id = Convert.ToInt64(split[0]);
            int rank = Convert.ToInt32(split[1]);
            if (rank > 60 || rank == 56 || rank < 0 || player_id <= 0)
                return Translation.GetLabel("ChangePlyRankWrongValue");
            else
            {
                Account pE = AccountManager.getAccount(player_id, 0);
                if (pE != null)
                {
                    if (ComDiv.updateDB("accounts", "rank", rank, "player_id", pE.player_id))
                    {
                        RankModel model = RankXML.getRank(rank);
                        pE._rank = rank;
                        SEND_ITEM_INFO.LoadGoldCash(pE);
                        pE.SendPacket(new BASE_RANK_UP_PAK(pE._rank, model != null ? model._onNextLevel : 0), false);
                        if (pE._room != null)
                            pE._room.updateSlotsInfo();
                        return Translation.GetLabel("ChangePlyRankSuccess", rank);
                    }
                    return Translation.GetLabel("ChangePlyRankFailUnk");
                }
                return Translation.GetLabel("ChangePlyRankFailPlayer");
            }
        }
    }
}
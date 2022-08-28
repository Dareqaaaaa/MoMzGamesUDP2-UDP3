﻿/*
 * Arquivo: VisitBox.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 04/12/2017
 * Sintam inveja, não nos atinge
 */

namespace Core.models.account
{
    public class VisitBox
    {
        public VisitItem reward1, reward2;
        public int RewardCount;
        public VisitBox()
        {
            reward1 = new VisitItem();
            reward2 = new VisitItem();
        }
        public void SetCount()
        {
            if (reward1 != null && reward1.good_id > 0) RewardCount++;
            if (reward2 != null && reward2.good_id > 0) RewardCount++;
        }
    }
    public class VisitItem
    {
        public int good_id, count;
        public bool IsReward;
        public void SetCount(string text)
        {
            count = int.Parse(text);
            if (count > 0)
                IsReward = true;
        }
    }
}
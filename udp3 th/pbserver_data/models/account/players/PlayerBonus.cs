/*
 * Arquivo: PlayerBonus.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 26/11/2017
 * Sintam inveja, não nos atinge
 */

namespace Core.models.account.players
{
    public class PlayerBonus
    {
        public int bonuses, sightColor = 4, freepass, fakeRank = 55;
        public string fakeNick = "";
        public long ownerId;
        public bool RemoveBonuses(int itemId)
        {
            int Dbonuses = bonuses, Dfreepass = freepass;
            if (itemId == 1200001000) Decrease(1); //Exp 10%
            else if (itemId == 1200002000) Decrease(2); //Exp 30%
            else if (itemId == 1200003000) Decrease(4); //Exp 50%
            else if (itemId == 1200037000) Decrease(8); //Exp 100%
            else if (itemId == 1200004000) Decrease(32); //Gold 30%
            else if (itemId == 1200119000) Decrease(64); //Gold 50%
            else if (itemId == 1200038000) Decrease(128); //Gold 100%
            else if (itemId == 1200011000) freepass = 0;
            return (bonuses != Dbonuses || freepass != Dfreepass);
        }
        public bool AddBonuses(int itemId)
        {
            int Dbonuses = bonuses, Dfreepass = freepass;
            if (itemId == 1200001000) Increase(1);
            else if (itemId == 1200002000) Increase(2);
            else if (itemId == 1200003000) Increase(4);
            else if (itemId == 1200037000) Increase(8);
            else if (itemId == 1200004000) Increase(32);
            else if (itemId == 1200119000) Increase(64);
            else if (itemId == 1200038000) Increase(128);
            else if (itemId == 1200011000) freepass = 1;
            return (bonuses != Dbonuses || freepass != Dfreepass);
        }
        private void Decrease(int value)
        {
            bonuses &= ~value;
        }
        private void Increase(int value)
        {
            bonuses |= value;
        }
    }
}
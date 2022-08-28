/*
 * Arquivo: RandomBoxItem.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 15/11/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.players;

namespace Core.models.randombox
{
    public class RandomBoxItem
    {
        public int index, percent;
        public uint count;
        public bool special;
        public ItemsModel item;
    }
}
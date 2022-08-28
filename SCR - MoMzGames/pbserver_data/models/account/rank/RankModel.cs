/*
 * Arquivo: RankModel.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

namespace Core.models.account.rank
{
    public class RankModel
    {
        public int _onNextLevel, _id, _onGPUp, _onAllExp;
        public RankModel(int rank, int onNextLevel, int onGPUp, int onAllExp)
        {
            _id = rank;
            _onNextLevel = onNextLevel;
            _onGPUp = onGPUp;
            _onAllExp = onAllExp;
        }
    }
}
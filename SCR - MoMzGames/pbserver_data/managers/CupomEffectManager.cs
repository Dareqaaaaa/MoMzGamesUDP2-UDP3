/*
 * Arquivo: CupomEffectManager.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 15/11/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.enums.flags;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.managers
{
    public static class CupomEffectManager
    {
        private static List<CupomFlag> Effects = new List<CupomFlag>();
        public static void LoadCupomFlags()
        {
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM info_cupons_flags";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        CupomFlag cupom = new CupomFlag
                        {
                            ItemId = data.GetInt32(0),
                            EffectFlag = (CupomEffects)data.GetInt64(1)
                        };
                        Effects.Add(cupom);
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }
        public static CupomFlag getCupomEffect(int id)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                CupomFlag flag = Effects[i];
                if (flag.ItemId == id)
                    return flag;
            }
            return null;
        }
    }

    public class CupomFlag
    {
        public int ItemId;
        public CupomEffects EffectFlag;
    }
}
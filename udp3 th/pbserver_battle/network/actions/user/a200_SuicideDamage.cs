using Battle.data.enums.weapon;
using SharpDX;
using System.Collections.Generic;

namespace Battle.network.actions.user
{
    public class a200_SuicideDamage
    {
        /// <summary>
        /// Puxa todas as informações. OnlyBytes desativado.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="genLog"></param>
        /// <param name="OnlyBytes">Não calcular informações adicionais?</param>
        /// <returns></returns>
        public static List<HitData> ReadInfo(ReceivePacket p, bool genLog, bool OnlyBytes = false)
        {
            return BaseReadInfo(p, OnlyBytes, genLog);
        }
        private static List<HitData> BaseReadInfo(ReceivePacket p, bool OnlyBytes, bool genLog)
        {
            List<HitData> hits = new List<HitData>();
            int count = p.readC();
            for (int i = 0; i < count; i++)
            {
                HitData hit = new HitData
                {
                    _hitInfo = p.readUD(),
                    _weaponInfo = p.readUH(),
                    _weaponSlot = p.readC(),
                    PlayerPos = p.readUHVector()
                };
                if (!OnlyBytes)
                {
                    hit.WeaponClass = (ClassType)((hit._weaponInfo >> 32) & 63); //Funcional? Antigo = >> 32) & 31 | Novo = >> 32) & 63
                    hit.WeaponId = (hit._weaponInfo >> 6);
                }
                if (genLog)
                {
                    Logger.warning("[" + i + "] Committed suicide: hitinfo,weaponinfo,weaponslot,camX,camY,camZ (" + hit._hitInfo + ";" + hit._weaponInfo + ";" + hit._weaponSlot + ";" + hit.PlayerPos.X + ";" + hit.PlayerPos.Y + ";" + hit.PlayerPos.Z + ")");
                }
                hits.Add(hit);
            }
            return hits;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            int count = p.readC();
            p.Advance(13 * count);
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            List<HitData> hits = ReadInfo(p, genLog, true);
            writeInfo(s, hits);
            hits = null;
        }
        public static void writeInfo(SendPacket s, List<HitData> hits)
        {
            s.writeC((byte)hits.Count);
            for (int i = 0; i < hits.Count; i++)
            {
                HitData hit = hits[i];
                s.writeD(hit._hitInfo);
                s.writeH(hit._weaponInfo);
                s.writeC(hit._weaponSlot);
                s.writeHVector(hit.PlayerPos);
            }
        }
        public class HitData
        {
            public uint _hitInfo;
            public ushort _weaponInfo;
            public Half3 PlayerPos;
            public byte _weaponSlot;
            public ClassType WeaponClass;
            public int WeaponId;
        }
    }
}
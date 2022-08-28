using Battle.data;
using Battle.data.enums;
using Battle.data.enums.weapon;
using SharpDX;
using System;
using System.Collections.Generic;

namespace Battle.network.actions.user
{
    public class a8000_NormalHitData
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
        public static void ReadInfo(ReceivePacket p)
        {
            int objsCount = p.readC();
            p.Advance(33 * objsCount);
        }
        private static List<HitData> BaseReadInfo(ReceivePacket p, bool OnlyBytes, bool genLog)
        {
            List<HitData> hits = new List<HitData>();
            int objsCount = p.readC();
            for (int ob = 0; ob < objsCount; ob++)
            {
                HitData hit = new HitData
                {
                    _hitInfo = p.readUD(),
                    _boomInfo = p.readUH(),
                    _weaponInfo = p.readUH(),
                    _weaponSlot = p.readC(), //EQMIPEMENT_SLOT
                    StartBullet = p.readTVector(),
                    EndBullet = p.readTVector()
                };
                if (!OnlyBytes)
                {
                    hit.HitEnum = (HitType)AllUtils.getHitHelmet(hit._hitInfo);
                    if (hit._boomInfo > 0)
                    {
                        hit.BoomPlayers = new List<int>();
                        for (int s = 0; s < 16; s++)
                        {
                            int flag = (1 << s);
                            if ((hit._boomInfo & flag) == flag)
                                hit.BoomPlayers.Add(s);
                        }
                    }
                    hit.WeaponClass = (ClassType)(hit._weaponInfo & 63);
                    hit.WeaponId = (hit._weaponInfo >> 6);
                }
                if (genLog)
                {
                    //Logger.warning("StartBulletAxis: " + hit._startBulletX + ";" + hit._startBulletY + ";" + hit._startBulletZ);
                    //Logger.warning("EndBulletAxis: " + hit._endBulletX + ";" + hit._endBulletY + ";" + hit._endBulletZ);
                }
                hits.Add(hit);
            }
            return hits;
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
                s.writeH(hit._boomInfo);
                s.writeH(hit._weaponInfo);
                s.writeC(hit._weaponSlot);
                s.writeTVector(hit.StartBullet);
                s.writeTVector(hit.EndBullet);
            }
        }
        public class HitData
        {
            public byte _weaponSlot;
            public ushort _boomInfo, _weaponInfo;
            public uint _hitInfo;
            public int WeaponId;
            public Half3 StartBullet, EndBullet;
            public List<int> BoomPlayers;
            public HitType HitEnum;
            public ClassType WeaponClass;
        }
    }
}
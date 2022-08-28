using System;
using System.Collections.Generic;

namespace Battle.network.actions.user
{
    public class a4000_BotHitData
    {
        public static void ReadInfo(ReceivePacket p)
        {
            int objsCount = p.readC();
            p.Advance(15 * objsCount);
        }
        public static List<HitData> ReadInfo(ReceivePacket p, bool genLog)
        {
            List<HitData> hits = new List<HitData>();
            int count = p.readC();
            for (int k = 0; k < count; k++)
            {
                HitData hit = new HitData
                {
                    _hitInfo = p.readUD(),
                    _weaponInfo = p.readUH(),
                    _weaponSlot = p.readC(),
                    _unk = p.readUH(),
                    _eixoX = p.readUH(),
                    _eixoY = p.readUH(),
                    _eixoZ = p.readUH()
                };
                if (genLog)
                {
                    Logger.warning("P: " + hit._eixoX + ";" + hit._eixoY + ";" + hit._eixoZ);
                    Logger.warning("[" + k + "] 16384: " + BitConverter.ToString(p.getBuffer()));
                }
                hits.Add(hit);
            }
            return hits;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            List<HitData> hits = ReadInfo(p, genLog);
            s.writeC((byte)hits.Count);
            for (int i = 0; i < hits.Count; i++)
            {
                HitData hit = hits[i];
                s.writeD(hit._hitInfo);
                s.writeH(hit._weaponInfo);
                s.writeC(hit._weaponSlot);
                s.writeH(hit._unk);
                s.writeH(hit._eixoX);
                s.writeH(hit._eixoY);
                s.writeH(hit._eixoZ);
            }
            hits = null;
        }
        public class HitData
        {
            public byte _weaponSlot;
            public ushort _weaponInfo, _eixoX, _eixoY, _eixoZ, _unk;
            public uint _hitInfo;
        }
    }
}
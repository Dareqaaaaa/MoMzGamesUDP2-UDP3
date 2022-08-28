using Battle.config;
using Battle.data.enums.weapon;
using System;

namespace Battle.network.actions.user
{
    public class a1000_DropWeapon
    {
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _weaponFlag = p.readC(),
                _weaponClass = p.readC(),
                _weaponId = p.readUH(),
                _ammoPrin = p.readC(),
                _ammoDual = p.readC(),
                _ammoTotal = p.readUH()
            };
            if (genLog)
            {
                Logger.warning("[ActionBuffer]: " + BitConverter.ToString(p.getBuffer()));
                Logger.warning("[DropWeapon] Flag: " + info._weaponFlag + "; Class: " + (ClassType)info._weaponClass + "; Id: " + info._weaponId);
            }
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(8);
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog, int count)
        {
            Struct info = ReadInfo(p, genLog);
            s.writeC((byte)(info._weaponFlag + count));
            s.writeC(info._weaponClass);
            s.writeH(info._weaponId);
            if (Config.useMaxAmmoInDrop)
            {
                s.writeC(255);
                s.writeC(info._ammoDual);
                s.writeH(10000);
            }
            else
            {
                s.writeC(info._ammoPrin);
                s.writeC(info._ammoDual);
                s.writeH(info._ammoTotal);
            }
            info = null;
        }
        public class Struct
        {
            public byte _weaponFlag, _weaponClass, _ammoPrin, _ammoDual;
            public ushort _weaponId, _ammoTotal;
        }
    }
}
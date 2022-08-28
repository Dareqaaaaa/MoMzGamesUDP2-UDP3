using Battle.config;
using Battle.data.enums.weapon;
using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a800_WeaponAmmo
    {
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
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
                Logger.warning("Slot " + ac._slot + " sync weapon ammo: wFl,wCl,wId,ammoP,ammoD,ammoT (" + info._weaponFlag + ";" + (ClassType)info._weaponClass + ";" + info._weaponId + ";" + info._ammoPrin + ";" + info._ammoDual + ";" + info._ammoTotal + ")");
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(8);
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(ac, p, genLog);
            s.writeC(info._weaponFlag);
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
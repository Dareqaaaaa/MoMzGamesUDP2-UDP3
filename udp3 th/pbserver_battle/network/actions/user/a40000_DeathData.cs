using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a40000_DeathData
    {
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _deathType = p.readC(),
                _hitPart = p.readC(),
                _camX = p.readUH(),
                _camY = p.readUH(),
                _camZ = p.readUH(),
                _weaponId = p.readD()
            };
            if (genLog)
                Logger.warning(ac._slot + " | " + info._deathType + ";" + info._hitPart + ";" + info._camX + ";" + info._camY + ";" + info._camZ + ";" + info._weaponId);
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(12);
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(ac, p, genLog);
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeC(info._deathType);
            s.writeC(info._hitPart);
            s.writeH(info._camX);
            s.writeH(info._camY);
            s.writeH(info._camZ);
            s.writeD(info._weaponId);
        }
        public class Struct
        {
            public byte _deathType, _hitPart;
            public ushort _camX, _camY, _camZ;
            public int _weaponId;
        }
    }
}
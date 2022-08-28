using System;

namespace Battle.network.actions.user
{
    public class a2000_FireSync
    {
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _shotId = p.readUH(),
                _shotIndex = p.readUH(),
                _camX = p.readUH(),
                _camY = p.readUH(),
                _camZ = p.readUH(),
                _weaponNumber = p.readD(), //weaponId
            };
            //Logger.warning("P: " + BitConverter.ToString(p.getBuffer()));
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(14);
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog);
            s.writeH(info._shotId);
            s.writeH(info._shotIndex);
            s.writeH(info._camX);
            s.writeH(info._camY);
            s.writeH(info._camZ);
            s.writeD(info._weaponNumber);
            info = null;
        }
        public class Struct
        {
            public ushort _shotId, _shotIndex, _camX, _camY, _camZ;
            public int _weaponNumber;
        }
    }
}
using SharpDX;
using System;

namespace Battle.network.actions.others
{
    public class code2_WeaponSync
    {
        public static byte[] ReadInfo(ReceivePacket p)
        {
            return p.readB(15);
        }
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _weaponFlag = p.readC(),
                _posX = p.readUH(),
                _posY = p.readUH(),
                _posZ = p.readUH(),
                _unk4 = p.readUH(),
                _unk5 = p.readUH(),
                _unk6 = p.readUH(),
                _unk7 = p.readUH()
            };
            if (genLog)
            {
                Logger.warning("[code2_WeaponSync] " + BitConverter.ToString(p.getBuffer()));
                Logger.warning("[code2_WeaponSync] Flag: " + info._weaponFlag + "; u4: " + info._unk4 + "; u5: " + info._unk5 + "; u6: " + info._unk6 + "; u7: " + info._unk7);
                //Vector3 vec = new Half3(info._posX, info._posY, info._posZ);
                //Logger.warning("[code2_WeaponSync] X: " + vec.X + "; Y: " + vec.Y + "; Z: " + vec.Z);
            }
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p)
        {
            s.writeB(ReadInfo(p));
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog);
            s.writeC(info._weaponFlag);
            s.writeH(info._posX);
            s.writeH(info._posY);
            s.writeH(info._posZ);
            s.writeH(info._unk4);
            s.writeH(info._unk5);
            s.writeH(info._unk6);
            s.writeH(info._unk7);
            info = null;
        }
        public class Struct
        {
            public byte _weaponFlag;
            public ushort _posX, _posY, _posZ, _unk4, _unk5, _unk6, _unk7;
        }
    }
}
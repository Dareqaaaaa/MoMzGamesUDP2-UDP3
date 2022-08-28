using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a80_WeaponRecoil
    {
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _RecoilHorzAngle = p.readT(),
                _RecoilHorzMax = p.readT(),
                _RecoilVertAngle = p.readT(),
                _RecoilVertMax = p.readT(),
                _Deviation = p.readT(),
                _weaponId = p.readUH(), //weaponId / 64
                _weaponSlot = p.readC(), //weaponSlot
                _unkV = p.readC(), //ping?
                _RecoilHorzCount = p.readC()
            };
            if (genLog)
                Logger.warning("Slot " + ac._slot + " weapon info: (" + info._RecoilHorzAngle + ";" + info._RecoilHorzMax + ";" + info._RecoilVertAngle + ";" + info._RecoilVertMax + ";" + info._Deviation + ";" + info._weaponId + ";" + info._weaponSlot + ";" + info._unkV + ";" + info._RecoilHorzCount + ")");
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(25);
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(ac, p, genLog);
            s.writeT(info._RecoilHorzAngle);
            s.writeT(info._RecoilHorzMax);
            s.writeT(info._RecoilVertAngle);
            s.writeT(info._RecoilVertMax);
            s.writeT(info._Deviation);
            s.writeH(info._weaponId);
            s.writeC(info._weaponSlot);
            s.writeC(info._unkV);
            s.writeC(info._RecoilHorzCount);
            info = null;
        }
        public class Struct
        {
            public float _RecoilHorzAngle, _RecoilHorzMax, _RecoilVertAngle, _RecoilVertMax, _Deviation;
            public ushort _weaponId;
            public byte _weaponSlot, _unkV, _RecoilHorzCount;
        }
    }
}
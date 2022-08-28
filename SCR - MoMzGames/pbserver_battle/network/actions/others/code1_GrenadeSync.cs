using System;

namespace Battle.network.actions.others
{
    public class code1_GrenadeSync
    {
        /// <summary>
        /// Puxa todas as informações. OnlyBytes desativado.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="genLog"></param>
        /// <param name="OnlyBytes">Não calcular informações adicionais?</param>
        /// <returns></returns>
        public static Struct ReadInfo(ReceivePacket p, bool genLog, bool OnlyBytes = false)
        {
            return BaseReadInfo(p, OnlyBytes, genLog);
        }
        private static Struct BaseReadInfo(ReceivePacket p, bool OnlyBytes, bool genLog)
        {
            Struct info = new Struct
            {
                _weaponInfo = p.readUH(),
                _weaponSlot = p.readC(),
                _unk = p.readUH(),
                _objPos_x = p.readUH(),
                _objPos_y = p.readUH(),
                _objPos_z = p.readUH(),
                _unk5 = p.readUH(),
                _unk6 = p.readUH(),
                _unk7 = p.readUH(),
                _grenadesCount = p.readUH()

            }; //id do item do mapa
          info._unk8 = p.readB(6);//Null 1.15.37
            /*
             * 1.15.42 - p.readB(6);
             */
            if (!OnlyBytes)
            {
                info.WeaponNumber = (info._weaponInfo >> 6);
                info.WeaponClass = (info._weaponInfo & 47);
            }
            if (genLog)
            {
                Logger.warning("[code1_GrenadeSync] " + BitConverter.ToString(p.getBuffer()));
                Logger.warning("[code1_GrenadeSync] wInfo: " + info._weaponInfo + "; wSlot: " + info._weaponSlot + "; u: " + info._unk + "; obpX: " + info._objPos_x + "; obpY: " + info._objPos_y + "; obpZ: " + info._objPos_z + "; u5: " + info._unk5 + "; u6: " + info._unk6 + "; u7: " + info._unk7 + "; u8: " + info._unk8);
            }
            return info;
        }
        public static byte[] ReadInfo(ReceivePacket p)
        {
            return p.readB(25);//19 = 1.15.37 | 25 = 1.15.42
        }
        public static void writeInfo(SendPacket s, ReceivePacket p)
        {
            s.writeB(ReadInfo(p));
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog, true);
            s.writeH(info._weaponInfo);
            s.writeC(info._weaponSlot);
            s.writeH(info._unk);
            s.writeH(info._objPos_x);
            s.writeH(info._objPos_y);
            s.writeH(info._objPos_z);
            s.writeH(info._unk5);
            s.writeH(info._unk6);
            s.writeH(info._unk7);
            s.writeH(info._grenadesCount);
            s.writeB(info._unk8);//Null 1.15.37
            /*
             * 1.15.42 - writeB(new byte[6]);
             */
            info = null;
        }
        public class Struct
        {
            public int WeaponNumber, WeaponClass;
            public ushort _weaponInfo, _objPos_x, _objPos_y, _objPos_z, _unk, _unk5, _unk6, _unk7, _grenadesCount;
            public byte _weaponSlot;
            public byte[] _unk8;
        }
    }
}
using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a40_WeaponSync
    {
        /// <summary>
        /// Puxa todas as informações. OnlyBytes desativado.
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="p"></param>
        /// <param name="genLog"></param>
        /// <param name="OnlyBytes">Não calcular informações adicionais?</param>
        /// <returns></returns>
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog, bool OnlyBytes = false)
        {
            return BaseReadInfo(ac, p, OnlyBytes, genLog);
        }
        private static Struct BaseReadInfo(ActionModel ac, ReceivePacket p, bool OnlyBytes, bool genLog)
        {
            Struct info = new Struct
            {
                _weaponInfo = p.readUH(),
                _weaponSlotInfo = p.readC(),
                _charaModelId = p.readC()
            };
            if (!OnlyBytes)
            {
                info.WeaponSecondMelee = (info._weaponSlotInfo >> 4);
                info.WeaponSlot = (info._weaponSlotInfo & 15);
                info.WeaponId = ((info._weaponInfo >> 6) & 1023);
                info.WeaponClass = (info._weaponInfo & 63);
            }
            if (genLog)
            {
                //Logger.warning("Slot " + aM._slot + " weapon sync: wInfo,ID,wSlot,charaModel (" + info._weaponInfo + ";" + (info._weaponInfo >> 6) + ";" + info._weaponSlot + ";" + info._charaModelId + ")");
            }
            return info;
        }
        /// <summary>
        /// Escreve os dados para envio.
        /// <para>Retorna True em caso de não uso do evento 64.</para>
        /// <para>Retorna False caso a ação 64 seja usada.</para>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ac"></param>
        /// <param name="p"></param>
        /// <param name="genLog"></param>
        /// <returns></returns>
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(ac, p, genLog, true);
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeH(info._weaponInfo);
            s.writeC(info._weaponSlotInfo);
            s.writeC(info._charaModelId); //VALOR > 0 && VALOR < 43
        }
        public class Struct
        {
            public ushort _weaponInfo;
            public byte _weaponSlotInfo, _charaModelId;
            public int WeaponClass, WeaponId, WeaponSlot, WeaponSecondMelee;
        }
    }
}
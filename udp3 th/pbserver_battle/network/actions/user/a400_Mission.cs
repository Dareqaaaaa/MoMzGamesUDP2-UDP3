using Battle.data.enums.bomb;
using Battle.data.models;
using Battle.data.sync;

namespace Battle.network.actions.user
{
    public class a400_Mission
    {
        /// <summary>
        /// Puxa todas as informações. OnlyBytes desativado.
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="p"></param>
        /// <param name="genLog"></param>
        /// <param name="time"></param>
        /// <param name="OnlyBytes">Não calcular informações adicionais?</param>
        /// <returns></returns>
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog, float time, bool OnlyBytes = false)
        {
            Struct info = new Struct
            {
                _bombAll = p.readC(),
                _plantTime = p.readT()
            };
            if (!OnlyBytes)
            {
                info.BombEnum = (BombFlag)(info._bombAll & 15);
                info.BombId = (info._bombAll >> 4);
            }
            if (genLog)
            {
                Logger.warning("Slot " + ac._slot + " bomb: (" + info.BombEnum + "; Id: " + info.BombId + "; sTime: " + info._plantTime + "; aTime: " + time + ")");
            }
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(5);
        }
        public static void SendC4UseSync(Room room, Player pl, Struct info)
        {
            if (pl == null) //?????????
                return;
            int type = info.BombEnum.HasFlag(BombFlag.Defuse) ? 1 : 0;
            Battle_SyncNet.SendBombSync(room, pl, type, info.BombId);
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog, float pacDate, float plantDuration)
        {
            Struct info = ReadInfo(ac, p, genLog, pacDate);
            if (info._plantTime > 0 && pacDate >= info._plantTime + (plantDuration) && !info.BombEnum.HasFlag(BombFlag.Stop))
                info._bombAll += 2;
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeC((byte)info._bombAll);
            s.writeT(info._plantTime);
        }
        public class Struct
        {
            public int _bombAll, BombId;
            public BombFlag BombEnum;
            public float _plantTime;
        }
    }
}
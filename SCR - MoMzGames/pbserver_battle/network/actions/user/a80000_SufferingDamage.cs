using Battle.config;
using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a80000_SufferingDamage
    {
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _hitEffects = p.readC(), //&15 = Qm deu o dano | >>4 = Tipo do dano (CHARA_DEATH)
                _hitPart = p.readC() //Número do efeito??
            };
            //Logger.warning("524288: " + info._hitPart);
            if (genLog)
            {
                Logger.warning("[1] Effect: " + (info._hitEffects >> 4) + "; By slot: " + (info._hitEffects & 15));
                Logger.warning("[2] Slot " + ac._slot + " action 524288: " + info._hitEffects + ";" + info._hitPart);
            }
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(2);
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(ac, p, genLog);
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeC(info._hitEffects);
            s.writeC(info._hitPart);
        }
        public class Struct
        {
            public byte _hitEffects, _hitPart;
        }
    }
}
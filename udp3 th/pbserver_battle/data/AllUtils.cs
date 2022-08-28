using Battle.data.enums;
using Battle.data.enums.weapon;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Battle.data
{
    public class AllUtils
    {
        public static string gen5(string text)
        {
            using (var hmacMD5 = new HMACMD5(Encoding.UTF8.GetBytes("/x!a@r-$r%an¨.&e&+f*f(f(a)")))
            {
                byte[] data = hmacMD5.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));
                return sBuilder.ToString();
            }
        }
        public static float GetDuration(DateTime date)
        {
            return (float)((DateTime.Now - date).TotalSeconds);
        }
        public static int createItemId(int class1, int usage, int classtype, int number)
        {
            return (class1 * 100000000) + (usage * 1000000) + (classtype * 1000) + number;
        }
        public static int itemClass(ClassType cw)
        {
            int valor = 1;
            if (cw == ClassType.Assault)
                valor = 1;
            else if (cw == ClassType.SMG || cw == ClassType.DualSMG)
                valor = 2;
            else if (cw == ClassType.Sniper)
                valor = 3;
            else if (cw == ClassType.Shotgun || cw == ClassType.DualShotgun)
                valor = 4;
            else if (cw == ClassType.MG)
                valor = 5;
            else if (cw == ClassType.HandGun || cw == ClassType.DualHandGun || cw == ClassType.CIC)
                valor = 6;
            else if (cw == ClassType.Knife || cw == ClassType.DualKnife || cw == ClassType.Knuckle)
                valor = 7;
            else if (cw == ClassType.Throwing)
                valor = 8;
            else if (cw == ClassType.Item)
                valor = 9;
            else if (cw == ClassType.Dino)
                valor = 0;
            return valor;
        }
        public static ObjectType getHitType(uint info)
        {
            return (ObjectType)(info & 3); //Max=4
        }
        public static int getHitWho(uint info)
        {
            return (int)((info >> 2) & 511); //Max=512
        }
        public static int getHitPart(uint info)
        {
            return (int)((info >> 11) & 63); //>> 11 & 63 Max=64
        }
        public static ushort getHitDamageBOT(uint info) //Modo BOT
        {
            return (ushort)(info >> 20); //(Max=4096)
        }
        public static ushort getHitDamageNORMAL(uint info) //32768 e 65536
        {
            return (ushort)(info >> 21); //(Max=4096)
        }
        public static int getHitHelmet(uint info)
        {
            return (int)((info >> 17) & 7); //Max=8
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UniqueRoomId"></param>
        /// <param name="type">0 - RoomId; 1 - ChannelId; 2 - ServerId</param>
        /// <returns></returns>
        public static int GetRoomInfo(uint UniqueRoomId, int type)
        {
            if (type == 0)
                return (int)(UniqueRoomId & 0xfff);
            else if (type == 1)
                return (int)((UniqueRoomId >> 12) & 0xff);
            else if (type == 2)
                return (int)((UniqueRoomId >> 20) & 0xfff);
            return 0;
        }

        public static byte[] encrypt(byte[] data, int shift)
        {
            byte[] result = new byte[data.Length];
            Buffer.BlockCopy(data, 0, result, 0, result.Length);
            int length = result.Length;
            byte first = result[0];
            byte current;
            for (int i = 0; i < length; i++)
            {
                current = i >= (length - 1) ? first : result[i + 1];
                result[i] = (byte)(current >> (8 - shift) | (result[i] << shift));
            }
            return result;
        }
        public static byte[] decrypt(byte[] data, int shift)
        {
            try
            {
                byte[] result = new byte[data.Length];
                Buffer.BlockCopy(data, 0, result, 0, result.Length);
                int length = result.Length;
                byte last = result[length - 1];
                byte current;
                for (int i = length - 1; (i & 0x80000000) == 0; i--)
                {
                    current = i <= 0 ? last : result[i - 1];
                    result[i] = (byte)(current << (8 - shift) | result[i] >> shift);
                }
                return result;
            }
            catch { Logger.warning(BitConverter.ToString(data)); return new byte[0]; }
        }

        public static int percentage(int total, int percent)
        {
            return total * percent / 100;
        }
        public static float percentage(float total, int percent)
        {
            return total * percent / 100;
        }
    }
}
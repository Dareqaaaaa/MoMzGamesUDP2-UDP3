using Battle.data;
using Battle.data.enums;
using Battle.data.models;
using Battle.network.actions.others;
using System;
using System.IO;

namespace Battle.network.packets
{
    public class Packet132Creator
    {
        public static byte[] getBaseData132(byte[] data)
        {
            ReceivePacket p = new ReceivePacket(data);
            using (SendPacket s = new SendPacket())
            {
                s.writeT(p.readT());
                for (int i = 0; i < 16; i++)
                {
                    ActionModel ac = new ActionModel();
                    try
                    {
                        bool exception;
                        ac._type = (P2P_SUB_HEAD)p.readC(out exception);
                        if (exception) break;
                        ac._slot = p.readUH();
                        ac._lengthData = p.readUH();
                        if (ac._lengthData == 65535)
                            break;
                        s.writeC((byte)ac._type);
                        s.writeH(ac._slot);
                        s.writeH(ac._lengthData);
                        if (ac._type == P2P_SUB_HEAD.GRENADE)
                            code1_GrenadeSync.writeInfo(s, p);
                        else if (ac._type == P2P_SUB_HEAD.DROPEDWEAPON)
                            code2_WeaponSync.writeInfo(s, p);
                        else if (ac._type == P2P_SUB_HEAD.OBJECT_STATIC)
                            code3_ObjectStatic.writeInfo(s, p);
                        else if (ac._type == P2P_SUB_HEAD.OBJECT_ANIM)
                            code6_ObjectAnim.writeInfo(s, p);
                        else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_STATIC)
                            code9_StageInfoObjStatic.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_ANIM)
                            code12_StageObjAnim.writeInfo(s, p);
                        else if (ac._type == P2P_SUB_HEAD.CONTROLED_OBJECT)
                            code13_ControledObj.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.USER || ac._type == P2P_SUB_HEAD.STAGEINFO_CHARA)
                        {
                            ac._flags = (Events)p.readUD();
                            ac._data = p.readB(ac._lengthData - 9);
                            s.writeD((uint)ac._flags);
                            s.writeB(ac._data);
                            if (ac._data.Length == 0 && (uint)ac._flags != 0)
                                break;
                        }
                        else
                        {
                            Logger.warning("[New user packet type2 '" + ac._type + "' or '" + (int)ac._type + "']: " + BitConverter.ToString(data));
                            throw new Exception("Unknown action type2");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.warning("B: " + BitConverter.ToString(data));
                        Logger.warning(ex.ToString());
                        s.mstream = new MemoryStream();
                        break;
                    }
                }
                return s.mstream.ToArray();
            }
        }
        public static byte[] getCode132(byte[] data, DateTime time, int round, int slot)
        {
            using (SendPacket s = new SendPacket())
            {
                byte[] actions = getBaseData132(data);
                s.writeC(132); //opcode
                s.writeC((byte)slot); //slot
                s.writeT(AllUtils.GetDuration(time));
                s.writeC((byte)round);
                s.writeH((ushort)(13 + actions.Length)); //lengthActionData
                s.writeD(0);
                s.writeB(AllUtils.encrypt(actions, (13 + actions.Length) % 6 + 1));
                return s.mstream.ToArray();
            }
        }
    }
}
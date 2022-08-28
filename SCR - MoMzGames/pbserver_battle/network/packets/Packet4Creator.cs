using Battle.data;
using Battle.data.enums;
using Battle.data.models;
using System;
using System.Collections.Generic;

namespace Battle.network.packets
{
    public class Packet4Creator
    {
        /// <summary>
        /// Gera um código do protocolo 4 com os dados desencriptados.
        /// </summary>
        /// <param name="actions">Ações (Não-Encriptadas)</param>
        /// <param name="date">Data da sala</param>
        /// <param name="round">Rodada da partida</param>
        /// <param name="slot">Slot do jogador; 255 (Todos)</param>
        /// <returns></returns>
        public static byte[] getCode4(byte[] actions, DateTime date, int round, int slot)
        {
            byte[] encrypted = AllUtils.encrypt(actions, (13 + actions.Length) % 6 + 1);
            return BaseGetCode4(encrypted, date, round, slot);
        }
        /// <summary>
        /// Gera um código do protocolo 4 com os dados encriptados.
        /// </summary>
        /// <param name="actionsBuffer">Ações (Encriptadas)</param>
        /// <param name="date">Data da sala</param>
        /// <param name="round">Rodada da partida</param>
        /// <param name="slot">Slot do jogador; 255 (Todos)</param>
        /// <returns></returns>
        private static byte[] BaseGetCode4(byte[] actionsBuffer, DateTime date, int round, int slot)
        {
            using (SendPacket s = new SendPacket())
            {
                s.writeC(4);
                s.writeC((byte)slot);
                s.writeT(AllUtils.GetDuration(date));
                s.writeC((byte)round);
                s.writeH((ushort)(13 + actionsBuffer.Length));
                s.writeD(0);
                s.writeB(actionsBuffer);
                return s.mstream.ToArray();
            }
        }
        public static byte[] getCode4SyncData(List<ObjectHitInfo> objs)
        {
            using (SendPacket s = new SendPacket())
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    ObjectHitInfo obj = objs[i];
                    if (obj.syncType == 1)
                    {
                        if (obj.objSyncId == 0)
                        {
                            s.writeC((byte)P2P_SUB_HEAD.OBJECT_STATIC);
                            s.writeH((ushort)obj.objId);
                            s.writeH(8);
                            s.writeH((ushort)obj.objLife);
                            s.writeC((byte)obj.killerId);
                        }
                        else
                        {
                            s.writeC((byte)P2P_SUB_HEAD.OBJECT_ANIM);
                            s.writeH((ushort)obj.objId);
                            s.writeH(13);
                            s.writeH((ushort)obj.objLife);
                            s.writeC((byte)obj._animId1);
                            s.writeC((byte)obj._animId2);
                            s.writeT(obj._specialUse);
                        }
                    }
                    else if (obj.syncType == 2)
                    {
                        Events events = Events.LifeSync;
                        int tamanho = 11;
                        if (obj.objLife == 0)
                        {
                            events |= Events.Death;
                            tamanho += 12;
                        }
                        s.writeC((byte)P2P_SUB_HEAD.USER);
                        s.writeH((ushort)obj.objId);
                        s.writeH((ushort)tamanho);
                        s.writeD((uint)events);
                        s.writeH((ushort)obj.objLife);
                        if (events.HasFlag(Events.Death))
                        {
                            s.writeC((byte)(obj.deathType + (obj.objId * 16)));
                            s.writeC((byte)obj.hitPart);
                            s.writeH(0);//obj.Position.X.RawValue);
                            s.writeH(0);//obj.Position.Y.RawValue);
                            s.writeH(0);//obj.Position.Z.RawValue);
                            s.writeD(obj.weaponId);
                            //Logger.warning("[Death] X: " + obj.Position.X + "; Y: " + obj.Position.Y + "; Z: " + obj.Position.Z);
                        }
                    }
                    else if (obj.syncType == 3)
                    {
                        if (obj.objSyncId == 0)
                        {
                            s.writeC((byte)P2P_SUB_HEAD.STAGEINFO_OBJ_STATIC);
                            s.writeH((ushort)obj.objId);
                            s.writeH(6);
                            s.writeC((obj.objLife == 0));
                        }
                        else
                        {
                            s.writeC((byte)P2P_SUB_HEAD.STAGEINFO_OBJ_ANIM);
                            s.writeH((ushort)obj.objId);
                            s.writeH(14);
                            s.writeC((byte)obj._destroyState);
                            s.writeH((ushort)obj.objLife);
                            s.writeT(obj._specialUse);
                            s.writeC((byte)obj._animId1);
                            s.writeC((byte)obj._animId2);
                        }
                    }
                    else if (obj.syncType == 4)
                    {
                        s.writeC((byte)P2P_SUB_HEAD.STAGEINFO_CHARA);
                        s.writeH((ushort)obj.objId);
                        s.writeH(11);
                        s.writeD(256);
                        s.writeH((ushort)obj.objLife);
                    }
                    else if (obj.syncType == 5)
                    {
                        s.writeC((byte)P2P_SUB_HEAD.USER);
                        s.writeH((short)obj.objId);
                        s.writeH(11);
                        s.writeD(524288);
                        s.writeC((byte)(obj.killerId + ((int)obj.deathType * 16)));
                        s.writeC((byte)obj.objLife);
                    }
                }
                return s.mstream.ToArray();
            }
        }
    }
}
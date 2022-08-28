/*
 * Arquivo: BASE_USER_MESSAGES_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 22/04/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
    public class BASE_USER_MESSAGES_PAK : SendPacket
    {
        private int pageIdx;
        private List<Message> msgs;
        public BASE_USER_MESSAGES_PAK(int pageIdx, List<Message> msgs)
        {
            this.pageIdx = pageIdx;
            this.msgs = new List<Message>();
            int count = 0;
            for (int i = pageIdx * 25; i < msgs.Count; i++)
            {
                this.msgs.Add(msgs[i]);
                if (++count == 25)
                    break;
            }
        }

        public override void write()
        {
            writeH(421);
            writeC((byte)pageIdx); //PageIdx
            writeC((byte)msgs.Count); //Max=25
            for (int i = 0; i < msgs.Count; i++)
            {
                Message msg = msgs[i];
                writeD(msg.object_id);
                writeQ(msg.sender_id);
                writeC((byte)msg.type);
                writeC((byte)msg.state);
                writeC((byte)msg.DaysRemaining);
                writeD(msg.clanId);
            }
            for (int i = 0; i < msgs.Count; i++)
            {
                Message msg = msgs[i];
                writeC((byte)(msg.sender_name.Length + 1));
                writeC((byte)(msg.type == 5 || msg.type == 4 && (int)msg.cB != 0 ? 0 : (msg.text.Length + 1)));
                writeS(msg.sender_name, msg.sender_name.Length + 1);
                if (msg.type == 5 || msg.type == 4)
                {
                    if ((int)msg.cB >= 4 && (int)msg.cB <= 6)
                    {
                        writeC((byte)(msg.text.Length + 1));
                        writeC((byte)msg.cB);
                        writeS(msg.text, msg.text.Length);
                    }
                    else if ((int)msg.cB == 0)
                        writeS(msg.text, msg.text.Length + 1);
                    else
                    {
                        writeC(2);
                        writeH((short)msg.cB);
                    }
                }
                else
                    writeS(msg.text, msg.text.Length + 1);
            }
        }
    }
}
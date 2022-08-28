/*
 * Arquivo: A_3428_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 14/07/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.enums;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class A_3428_REC : ReceiveGamePacket
    {
        public A_3428_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            if (_client == null || _client._player == null) 
                return;
            try
            {
                Account player = _client._player;
                Room room = player._room;
                if (room != null && (int)room._state >= 2 && room._slots[player._slotId].state == SLOT_STATE.NORMAL)
                    _client.SendPacket(new A_3428_PAK(room));
            }
            catch
            {
            }
        }
    }
}
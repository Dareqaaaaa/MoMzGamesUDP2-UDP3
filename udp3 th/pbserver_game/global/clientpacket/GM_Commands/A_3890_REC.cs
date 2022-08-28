/*
 * Arquivo: A_3890_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 27/10/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class A_3890_REC : ReceiveGamePacket
    {
        private int Slot;
        public A_3890_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            Slot = readC();
        }

        public override void run()
        {
            if (_client == null)
                return;
            Account p = _client._player;
            if (p == null || p._slotId == Slot)
                return;
            if (!p.IsGM())
            {
                _client.Close(1000);
                return;
            }
            try
            {
                Room room = p._room;
                if (room == null)
                    return;
                Account pR = room.getPlayerBySlot(Slot);
                if (pR == null)
                    return;
                pR.SendPacket(new AUTH_ACCOUNT_KICK_PAK(2));
                pR.Close(1000, true);
                //Ativa quando usa "/KICK (slotid)"
                Logger.warning("[3890] Slot: " + Slot);
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
    }
}
using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class ROOM_CHANGE_PASSW_REC : ReceiveGamePacket
    {
        private string pass;
        public ROOM_CHANGE_PASSW_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            pass = readS(4);
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                    return;
                Room room = player._room;
                if (room != null && room._leader == player._slotId && room.password != pass)
                {
                    room.password = pass;
                    using (ROOM_CHANGE_PASSWD_PAK packet = new ROOM_CHANGE_PASSWD_PAK(pass))
                        room.SendPacketToPlayers(packet);
                }
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
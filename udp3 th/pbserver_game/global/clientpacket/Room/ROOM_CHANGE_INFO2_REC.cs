using Core;
using Core.models.enums;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class ROOM_CHANGE_INFO2_REC : ReceiveGamePacket
    {
        private string leader;
        public ROOM_CHANGE_INFO2_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && room._leader == player._slotId && room._state == RoomState.Ready)
                {
                    leader = readS(33);
                    room.killtime = readD();
                    room.limit = readC();
                    room.seeConf = readC();
                    room.autobalans = readH();
                    using (ROOM_CHANGE_INFO_PAK packet = new ROOM_CHANGE_INFO_PAK(room, leader))
                        room.SendPacketToPlayers(packet);
                }
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }

        public override void run()
        {
        }
    }
}
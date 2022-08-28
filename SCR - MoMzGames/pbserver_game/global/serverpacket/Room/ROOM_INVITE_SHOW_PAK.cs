using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class ROOM_INVITE_SHOW_PAK : SendPacket
    {
        private Account sender;
        private Room room;
        public ROOM_INVITE_SHOW_PAK(Account sender, Room room)
        {
            this.sender = sender;
            this.room = room;
        }

        public override void write()
        {
            writeH(2053);
            writeS(sender.player_name, 33);
            writeD(room._roomId);
            writeQ(sender.player_id);
            writeS(room.password, 4);
        }
    }
}
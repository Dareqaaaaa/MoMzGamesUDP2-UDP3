using Core;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class SendMsgToPlayers
    {
        public static string SendToAll(string str)
        {
            string msg = str.Substring(2);
            int count = 0;
            using (SERVER_MESSAGE_ANNOUNCE_PAK packet = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
                count = GameManager.SendPacketToAllClients(packet);
            return Translation.GetLabel("MsgAllClients", count);
        }

        public static string SendToRoom(string str, Room room)
        {
            string msg = str.Substring(3);
            if (room == null)
                return Translation.GetLabel("GeneralRoomInvalid");
            using (SERVER_MESSAGE_ANNOUNCE_PAK packet = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
                room.SendPacketToPlayers(packet);
            return Translation.GetLabel("MsgRoomPlayers");
        }
    }
}
using Core;
using Core.models.enums;
using Core.server;
using Game.data.model;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.data.sync.client_side
{
    public static class Net_Room_HitMarker
    {
        public static void Load(ReceiveGPacket p)
        {
            int roomId = p.readH();
            int channelId = p.readH();
            byte killerIdx = p.readC();
            byte deathtype = p.readC();
            byte hitEnum = p.readC();
            int damage = p.readH();
            if (p.getBuffer().Length > 11)
                Logger.warning("[Invalid MARKER: " + BitConverter.ToString(p.getBuffer()) + "]");
            Channel ch = ChannelsXML.getChannel(channelId);
            if (ch == null)
                return;
            Room room = ch.getRoom(roomId);
            if (room != null && room._state == RoomState.Battle)
            {
                Account player = room.getPlayerBySlot(killerIdx);
                if (player != null)
                {
                    string warn = "";
                    if (deathtype == 10)
                        warn = Translation.GetLabel("LifeRestored", damage);
                    else if (hitEnum == 0)
                        warn = Translation.GetLabel("HitMarker1", damage);
                    else if (hitEnum == 1)
                        warn = Translation.GetLabel("HitMarker2", damage);
                    else if (hitEnum == 2)
                        warn = Translation.GetLabel("HitMarker3");
                    else if (hitEnum == 3)
                        warn = Translation.GetLabel("HitMarker4");
                    player.SendPacket(new LOBBY_CHATTING_PAK(Translation.GetLabel("HitMarkerName"), player.getSessionId(), 0, true, warn));
                }
            }
        }
    }
}
using Core;
using Core.models.enums;
using Core.models.enums.flags;
using Game.data.model;

namespace Game.data.chat
{
    public static class ChangeRoomInfos
    {
        public static string ChangeMap(string str, Room room)
        {
            if (room == null)
                return Translation.GetLabel("GeneralRoomInvalid");
            if (!room.isStartingMatch())
            {
                int map = int.Parse(str.Substring(4));
                if (map <= 0)
                    return Translation.GetLabel("ChangeMapFail");
                else
                {
                    room.mapId = map;
                    room.updateRoomInfo();
                    return Translation.GetLabel("ChangeMapSuccess", room.mapId);
                }
            }
            else
                return Translation.GetLabel("ChangeMapRoomFail");
        }
        public static string ChangeMap2(string str, Room room)
        {
            if (room == null)
                return Translation.GetLabel("GeneralRoomInvalid");
            if (room != null)
            {
                int map = int.Parse(str.Substring(4));
                if (map <= 0)
                    return Translation.GetLabel("ChangeMapFail");
                else
                {
                    room.mapId = map;
                    room.updateRoomInfo();
                    return Translation.GetLabel("ChangeMapSuccess", room.mapId);
                }
            }
            else
                return Translation.GetLabel("ChangeMapRoomFail");
        }
        public static string ChangeTime(string str, Room room)
        {
            if (room == null)
                return Translation.GetLabel("GeneralRoomInvalid");
            int time = int.Parse(str.Substring(2));
            if (time < 0)
                return Translation.GetLabel("ChangeTimeWrong");
            else if (!room.isStartingMatch())
            {
                room.killtime = time;
                room.updateRoomInfo();
                return Translation.GetLabel("ChangeTimeSuccess", room.getTimeByMask(), room.getRoundsByMask(), room.getKillsByMask());
            }
            else
                return Translation.GetLabel("ChangeTimeRoomFail");
        }
        public static string ChangeTime2(string str, Room room)
        {
            if (room == null)
                return Translation.GetLabel("GeneralRoomInvalid");
            uint time = uint.Parse(str.Substring(2));
            if (time < 0)
                return Translation.GetLabel("ChangeTimeWrong");
            else if (room != null)
            {
                room._timeRoom = time;
                room.updateRoomInfo();
                return Translation.GetLabel("ChangeTimeSuccess", room.getTimeByMask(), room.getRoundsByMask(), room.getKillsByMask());
            }
            else
                return Translation.GetLabel("ChangeTimeRoomFail");
        }
        public static string ChangeStageType(string str, Room room)
        {
            int stageType = int.Parse(str.Substring(12));
            if (room != null)
            {
                room.room_type = (byte)stageType;
                room.updateRoomInfo();
                return Translation.GetLabel("ChangeStageTypeSuccess", (RoomType)stageType);
            }
            else
                return Translation.GetLabel("GeneralRoomInvalid");
        }
        public static string ChangeSpecialType(string str, Room room)
        {
            int special = int.Parse(str.Substring(15));
            if (room != null)
            {
                room.special = (byte)special;
                room.updateRoomInfo();
                return Translation.GetLabel("ChangeSpecialTypeSuccess", (RoomSpecial)special);
            }
            else
                return Translation.GetLabel("GeneralRoomInvalid");
        }
        public static string ChangeWeaponsFlag(string str, Room room)
        {
            int flags = int.Parse(str.Substring(12));
            if (room != null)
            {
                room.weaponsFlag = (byte)flags;
                room.updateRoomInfo();
                return Translation.GetLabel("ChangeWeaponsFlagSuccess", (RoomWeaponsFlag)flags);
            }
            else
                return Translation.GetLabel("GeneralRoomInvalid");
        }

        public static string UnlockById(string str, Account player)
        {
            int roomId = int.Parse(str.Substring(11));
            roomId--;
            if (player == null)
                return Translation.GetLabel("RoomUnlock_Fail3");

            Channel channel = player.getChannel();
            if (channel == null)
                return Translation.GetLabel("GeneralChannelInvalid");

            Room rm = channel.getRoom(roomId);
            if (rm != null && rm.limit == 1)
            {
                rm.limit = 0;
                rm.updateRoomInfo();
                return Translation.GetLabel("RoomUnlock_Success", string.Format("{0:0##}", roomId + 1));
            }
            else
                return Translation.GetLabel("RoomUnlock_Fail1", string.Format("{0:0##}", roomId + 1));
        }
    }
}
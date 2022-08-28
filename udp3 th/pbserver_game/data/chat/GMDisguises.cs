using Core;
using Core.managers;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class GMDisguises
    {
        public static string SetHideColor(Account player)
        {
            if (player == null)
                return Translation.GetLabel("HideGMColorFail");
            if (player._rank == 53 || player._rank == 54)
            {
                player.HideGMcolor = !player.HideGMcolor;
                if (player.HideGMcolor)
                    return Translation.GetLabel("HideGMColorSuccess1");
                else
                    return Translation.GetLabel("HideGMColorSuccess2");
            }
            return Translation.GetLabel("HideGMColorNoRank");
        }
        public static string SetAntiKick(Account player)
        {
            if (player == null)
                return Translation.GetLabel("AntiKickGMFail");
            player.AntiKickGM = !player.AntiKickGM;
            if (player.AntiKickGM)
                return Translation.GetLabel("AntiKickGMSuccess1");
            else
                return Translation.GetLabel("AntiKickGMSuccess2");
        }

        public static string SetFakeRank(string str, Account player, Room room)
        {
            int rank = int.Parse(str.Substring(9));
            if (rank > 55 || rank < 0)
                return Translation.GetLabel("FakeRankWrongValue");
            else if (player._bonus.fakeRank == rank)
                return Translation.GetLabel("FakeRankAlreadyFaked");
            else if (ComDiv.updateDB("player_bonus", "fakerank", rank, "player_id", player.player_id))
            {
                player._bonus.fakeRank = rank;
                player.SendPacket(new BASE_USER_EFFECTS_PAK(0, player._bonus));
                if (room != null)
                    room.updateSlotsInfo();
                if (rank == 55)
                    return Translation.GetLabel("FakeRankSuccess1");
                else
                    return Translation.GetLabel("FakeRankSuccess2", rank);
            }
            return Translation.GetLabel("FakeRankFail");
        }
        public static string SetFakeNick(string str, Account player, Room room)
        {
            string name = str.Substring(11);
           if (name.Length > ConfigGS.maxNickSize || name.Length < ConfigGS.minNickSize)
                return Translation.GetLabel("FakeNickWrongLength");
            else if (PlayerManager.isPlayerNameExist(name))
                return Translation.GetLabel("FakeNickAlreadyExist");
            else if (ComDiv.updateDB("accounts", "player_name", name, "player_id", player.player_id))
            {
                player.player_name = name;
                player.SendPacket(new AUTH_CHANGE_NICKNAME_PAK(name));
                if (room != null)
                    using (ROOM_GET_NICKNAME_PAK packet = new ROOM_GET_NICKNAME_PAK(player._slotId, player.player_name, player.name_color))
                        room.SendPacketToPlayers(packet);
                if (player.clanId > 0)
                {
                    using (CLAN_MEMBER_INFO_UPDATE_PAK packet = new CLAN_MEMBER_INFO_UPDATE_PAK(player))
                        ClanManager.SendPacket(packet, player.clanId, -1, true, true);
                }
                AllUtils.syncPlayerToFriends(player, true);
                return Translation.GetLabel("FakeNickSuccess", name);
            }
            return Translation.GetLabel("FakeNickFail");
        }
    }
}
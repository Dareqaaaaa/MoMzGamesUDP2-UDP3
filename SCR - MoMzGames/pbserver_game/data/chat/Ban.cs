using Core;
using Core.managers;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.data.chat
{
    public static class Ban
    {
        public static string UpdateReason(string str)
        {
            string text = str.Substring(7);
            int idx = text.IndexOf(" ");
            if (idx >= 0)
            {
                long object_id;
                string reason;
                string[] split = text.Split(' ');
                object_id = long.Parse(split[0]);
                reason = text.Substring(idx + 1);
                if (BanManager.SaveBanReason(object_id, reason))
                    return Translation.GetLabel("PlayerBanReasonSuccess");
                else
                    return Translation.GetLabel("PlayerBanReasonFail");
            }
            else
                return "Comando inválido. [Servidor]";
        }

        public static string BanForeverNick(string str, Account player, bool warn)
        {
            Account victim = AccountManager.getAccount(str.Substring(6), 1, 0);
            return BaseBanForever(player, victim, warn);
        }
        public static string BanForeverId(string str, Account player, bool warn)
        {
            Account victim = AccountManager.getAccount(long.Parse(str.Substring(7)), 0);
            return BaseBanForever(player, victim, warn);
        }

        public static string BanNormalNick(string str, Account player, bool warn)
        {
            string text = str.Substring(5);
            string[] split = text.Split(' ');
            string nick = split[0];
            double days = Convert.ToDouble(split[1]);
            DateTime endDate = DateTime.Now.AddDays(days);
            Account victim = AccountManager.getAccount(nick, 1, 0);
            return BaseBanNormal(player, victim, warn, endDate);
        }
        public static string BanNormalId(string str, Account player, bool warn)
        {
            string text = str.Substring(6);
            string[] split = text.Split(' ');
            long player_id = Convert.ToInt64(split[0]);
            double days = Convert.ToDouble(split[1]);
            DateTime endDate = DateTime.Now.AddDays(days);
            Account victim = AccountManager.getAccount(player_id, 0);
            return BaseBanNormal(player, victim, warn, endDate);
        }

        private static string BaseBanNormal(Account player, Account victim, bool warn, DateTime endDate)
        {
            if (victim == null)
                return Translation.GetLabel("PlayerBanUserInvalid");
            else if (victim.access > player.access)
                return Translation.GetLabel("PlayerBanAccessInvalid");
            else if (player.player_id == victim.player_id)
                return Translation.GetLabel("PlayerBanSimilarID");
            else
            {
                BanHistory ban = BanManager.SaveHistory(player.player_id, "DURATION", victim.player_id.ToString(), endDate);
                if (ban != null)
                {
                    if (warn)
                    {
                        using (SERVER_MESSAGE_ANNOUNCE_PAK packet = new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("PlayerBannedWarning2", victim.player_name)))
                            GameManager.SendPacketToAllClients(packet);
                    }
                    victim.ban_obj_id = ban.object_id;
                    victim.SendPacket(new AUTH_ACCOUNT_KICK_PAK(2), false);
                    victim.Close(1000, true);
                    return Translation.GetLabel("PlayerBanSuccess", ban.object_id);
                }
                else
                    return Translation.GetLabel("PlayerBanFail");
            }
        }
        private static string BaseBanForever(Account player, Account victim, bool warn)
        {
            if (victim == null)
                return Translation.GetLabel("PlayerBanUserInvalid");
            else if (victim.access > player.access)
                return Translation.GetLabel("PlayerBanAccessInvalid");
            else if (player.player_id == victim.player_id)
                return Translation.GetLabel("PlayerBanSimilarID");
            else if (ComDiv.updateDB("accounts", "access_level", -1, "player_id", victim.player_id))
            {
                if (warn)
                {
                    using (SERVER_MESSAGE_ANNOUNCE_PAK packet = new SERVER_MESSAGE_ANNOUNCE_PAK(Translation.GetLabel("PlayerBannedWarning", victim.player_name)))
                        GameManager.SendPacketToAllClients(packet);
                }
                victim.access = AccessLevel.Banned;
                victim.SendPacket(new AUTH_ACCOUNT_KICK_PAK(2), false);
                victim.Close(1000, true);
                return Translation.GetLabel("PlayerBanSuccess", -1);
            }
            else
                return Translation.GetLabel("PlayerBanFail");
        }

        public static string GetBanData(string str, Account player)
        {
            long id = long.Parse(str.Substring(7));
            BanHistory ban = BanManager.GetAccountBan(id);
            if (ban == null)
                return Translation.GetLabel("GetBanInfoError");
            string info = Translation.GetLabel("GetBanInfoTitle");
            info += "\n" + Translation.GetLabel("GetBanInfoProvider", ban.provider_id);
            info += "\n" + Translation.GetLabel("GetBanInfoType", ban.type);
            info += "\n" + Translation.GetLabel("GetBanInfoValue", ban.value);
            info += "\n" + Translation.GetLabel("GetBanInfoReason", ban.reason);
            info += "\n" + Translation.GetLabel("GetBanInfoStart", ban.startDate);
            info += "\n" + Translation.GetLabel("GetBanInfoEnd", ban.endDate);
            player.SendPacket(new SERVER_MESSAGE_ANNOUNCE_PAK(info));
            return Translation.GetLabel("GetBanInfoSuccess");
        }
    }
}
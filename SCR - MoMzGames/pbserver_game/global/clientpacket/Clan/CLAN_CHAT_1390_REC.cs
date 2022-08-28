using Core;
using Core.models.enums.global;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_CHAT_1390_REC : ReceiveGamePacket
    {
        private ChattingType type;
        private string text;
        public CLAN_CHAT_1390_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            type = (ChattingType)readH();
            text = readS(readH());
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || type != ChattingType.Clan_Member_Page)
                    return;
                serverCommands(p, p._room);
                using (CLAN_CHAT_1390_PAK packet = new CLAN_CHAT_1390_PAK(p, text))
                    ClanManager.SendPacket(packet, p.clanId, -1, true, true);
            }
            catch
            {
            }
        }
        private bool serverCommands(Account player, Room room)
        {
            try
            {
                if (!player.HaveGMLevel() || !(text.StartsWith(";") || text.StartsWith(@"\") || text.StartsWith(".")))
                    return false;
                Logger.LogCMD("[" + text + "] playerId: " + player.player_id + "; Nick: '" + player.player_name + "'; Login: '" + player.login + "'; Ip: '" + player.PublicIP.ToString() + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
                string str = text.Substring(1);
                if (str.StartsWith("vv "))
                {
                    ushort op = ushort.Parse(str.Substring(3));
                    _client.SendPacket(new HELPER_PAK(op));
                }
                else
                    text = Translation.GetLabel("UnknownCmd");
                return true;
            }
            catch (Exception ex)
            {
                Logger.warning("[CLAN_CHAT_1390_REC] " + ex.ToString());
                text = Translation.GetLabel("CrashProblemCmd");
                return true;
            }
        }
    }
}
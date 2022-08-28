using Core;
using Core.models.enums.global;
using Core.models.enums.match;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_TEAM_CHATTING_REC : ReceiveGamePacket
    {
        private ChattingType type;
        private string text;
        public CLAN_WAR_TEAM_CHATTING_REC(GameClient client, byte[] data)
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
                if (p == null || p._match == null || type != ChattingType.Match)
                    return;
                Match match = p._match;
                serverCommands(p, match);
                using (CLAN_WAR_TEAM_CHATTING_PAK packet = new CLAN_WAR_TEAM_CHATTING_PAK(p.player_name, text))
                    match.SendPacketToPlayers(packet);
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_WAR_TEAM_CHATTING_REC: " + ex.ToString());
            }
        }

        private bool serverCommands(Account player, Match match)
        {
            try
            {
                if (!player.HaveGMLevel() || !(text.StartsWith(";") || text.StartsWith(@"\") || text.StartsWith(".")))
                    return false;
                string str = text.Substring(1);
                if (str.StartsWith("o") && (int)player.access >= 3)
                {
                    if (match != null)
                    {
                        AccountManager.getAccountDB((long)2, 2);
                        for (int i = 0; i < match.formação; i++)
                        {
                            SLOT_MATCH slot = match._slots[i];
                            if (slot._playerId == 0)
                            {
                                slot._playerId = 2;
                                slot.state = SlotMatchState.Normal;
                            }
                        }
                        using (CLAN_WAR_REGIST_MERCENARY_PAK packet = new CLAN_WAR_REGIST_MERCENARY_PAK(match))
                            match.SendPacketToPlayers(packet);
                        text = "Disputa preenchida. [Servidor]";
                    }
                    else
                        text = "Falha ao encher a disputa. [Servidor]";
                }
                else if (str.StartsWith("gg"))
                {
                    match._state = MatchState.Play;
                    match._slots[player.matchSlot].state = SlotMatchState.Ready;
                    _client.SendPacket(new CLAN_WAR_ENEMY_INFO_PAK(match));
                    _client.SendPacket(new CLAN_WAR_CREATED_ROOM_PAK(match));
                }
                else
                    text = "Falha ao encontrar o comando digitado. [Servidor]";
                return true;
            }
            catch (OverflowException ex)
            { return true; }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
                return true;
            }
        }
    }
}
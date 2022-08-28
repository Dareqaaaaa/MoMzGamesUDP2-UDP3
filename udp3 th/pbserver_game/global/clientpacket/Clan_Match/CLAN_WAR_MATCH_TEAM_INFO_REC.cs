using Core;
using Game.data.model;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_MATCH_TEAM_INFO_REC : ReceiveGamePacket
    {
        private int id, serverInfo;
        public CLAN_WAR_MATCH_TEAM_INFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            id = readH();
            serverInfo = readH();
        }

        public override void run()
        {
            Account player = _client._player;
            if (player == null || player._match == null)
                return;
            try
            {
                int channelId = serverInfo - ((serverInfo / 10) * 10);
                Channel ch = ChannelsXML.getChannel(channelId);
                if (ch != null)
                {
                    Match match = ch.getMatch(id);
                    if (match != null)
                        _client.SendPacket(new CLAN_WAR_MATCH_TEAM_INFO_PAK(0, match.clan));
                    else
                        _client.SendPacket(new CLAN_WAR_MATCH_TEAM_INFO_PAK(0x80000000));
                }
                else
                    _client.SendPacket(new CLAN_WAR_MATCH_TEAM_INFO_PAK(0x80000000));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
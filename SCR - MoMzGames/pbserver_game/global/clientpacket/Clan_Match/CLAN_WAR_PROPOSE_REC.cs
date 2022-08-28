using Core;
using Core.models.enums.match;
using Game.data.model;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_PROPOSE_REC : ReceiveGamePacket
    {
        private int id, serverInfo;
        private uint erro;
        public CLAN_WAR_PROPOSE_REC(GameClient client, byte[] data)
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
            try
            {
                Account p = _client._player;
                if (p != null && p._match != null && p.matchSlot == p._match._leader && p._match._state == MatchState.Ready)
                {
                    int channelId = serverInfo - ((serverInfo / 10) * 10);
                    Match mt = ChannelsXML.getChannel(channelId).getMatch(id);
                    if (mt != null)
                    {
                        Account lider = mt.getLeader();
                        if (lider != null && lider._connection != null && lider._isOnline)
                            lider.SendPacket(new CLAN_WAR_MATCH_REQUEST_BATTLE_PAK(p._match, p));
                        else
                            erro = 0x80000000;
                    }
                    else erro = 0x80000000;
                }
                else erro = 0x80000000;
                _client.SendPacket(new CLAN_WAR_MATCH_PROPOSE_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_WAR_PROPOSE_REC: " + ex.ToString());
            }
        }
    }
}
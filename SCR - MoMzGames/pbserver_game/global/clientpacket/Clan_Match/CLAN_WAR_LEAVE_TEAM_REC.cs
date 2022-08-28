using Core;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_WAR_LEAVE_TEAM_REC : ReceiveGamePacket
    {
        private uint erro;
        public CLAN_WAR_LEAVE_TEAM_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                Match mt = p == null ? null : p._match;
                if (mt == null || !mt.RemovePlayer(p))
                    erro = 0x80000000;
                _client.SendPacket(new CLAN_WAR_LEAVE_TEAM_PAK(erro));
                if (erro == 0)
                {
                    p._status.updateClanMatch(255);
                    AllUtils.syncPlayerToClanMembers(p);
                }
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
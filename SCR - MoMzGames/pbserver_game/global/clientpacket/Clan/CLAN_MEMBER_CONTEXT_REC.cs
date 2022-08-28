/*
 * Arquivo: CLAN_MEMBER_CONTEXT_REC.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/02/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_MEMBER_CONTEXT_REC : ReceiveGamePacket
    {
        public CLAN_MEMBER_CONTEXT_REC(GameClient client, byte[] data)
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
                if (p == null)
                    return;
                int clanId = p.clanId;
                if (clanId == 0)
                    _client.SendPacket(new CLAN_MEMBER_CONTEXT_PAK(-1));
                else
                    _client.SendPacket(new CLAN_MEMBER_CONTEXT_PAK(0, PlayerManager.getClanPlayers(clanId)));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
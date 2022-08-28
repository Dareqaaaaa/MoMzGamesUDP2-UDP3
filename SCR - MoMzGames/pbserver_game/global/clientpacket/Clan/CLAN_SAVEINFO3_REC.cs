using Core;
using Core.managers;
using Core.models.account.clan;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_SAVEINFO3_REC : ReceiveGamePacket
    {
        private int limite_rank, limite_idade, limite_idade2, autoridade;
        private uint erro;
        public CLAN_SAVEINFO3_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            autoridade = readC();
            limite_rank = readC();
            limite_idade = readC();
            limite_idade2 = readC();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                    return;
                Clan c = ClanManager.getClan(player.clanId);
                if (c._id > 0 && (c.owner_id == _client.player_id) && PlayerManager.updateClanInfo(c._id, autoridade, limite_rank, limite_idade, limite_idade2))
                {
                    c.autoridade = autoridade;
                    c.limite_rank = limite_rank;
                    c.limite_idade = limite_idade;
                    c.limite_idade2 = limite_idade2;
                }
                else erro = 0x80000000;

                _client.SendPacket(new CLAN_SAVEINFO3_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}
/*
 * Arquivo: CLAN_CREATE_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/02/2017
 * Sintam inveja, não nos atinge
 */

using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_CREATE_PAK : SendPacket
    {
        private Account _p;
        private Clan clan;
        private uint _erro;
        public CLAN_CREATE_PAK(uint erro, Clan clan, Account player)
        {
            _erro = erro;
            this.clan = clan;
            _p = player;
        }

        public override void write()
        {
            writeH(1311);
            writeD(_erro);
            if (_erro == 0)
            {
                writeD(clan._id);
                writeS(clan._name, 17);
                writeC((byte)clan._rank);
                writeC((byte)PlayerManager.getClanPlayers(clan._id));
                writeC((byte)clan.maxPlayers);
                writeD(clan.creationDate);
                writeD(clan._logo);
                writeB(new byte[10]);
                writeQ(clan.owner_id);
                writeS(_p.player_name, 33);
                writeC((byte)_p._rank);
                writeS(clan._info, 255);
                writeS("Temp", 21);
                writeC((byte)clan.limite_rank);
                writeC((byte)clan.limite_idade);
                writeC((byte)clan.limite_idade2);
                writeC((byte)clan.autoridade);
                writeS("", 255);
                writeB(new byte[104]);
                writeT(clan._pontos);
                writeD(_p._gp);
            }
        }
    }
}
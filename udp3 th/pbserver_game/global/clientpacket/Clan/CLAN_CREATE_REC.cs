using Core;
using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_CREATE_REC : ReceiveGamePacket
    {
        private int NameLength, InfoLength, AzitLength;
        private uint erro;
        private string clanName, clanInfo, clanAzit;
        public CLAN_CREATE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            NameLength = readC();
            InfoLength = readC();
            AzitLength = readC();
            clanName = readS(NameLength);
            clanInfo = readS(InfoLength);
            clanAzit = readS(AzitLength);
            readD();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                Clan clan = new Clan
                {
                    _name = clanName,
                    _info = clanInfo,
                    _logo = 0,
                    owner_id = p.player_id,
                    creationDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"))
                };
                if (p.clanId > 0 || PlayerManager.getRequestClanId(p.player_id) > 0)
                    erro = 0x8000105C;
                else if (0 > p._gp - ConfigGS.minCreateGold || ConfigGS.minCreateRank > p._rank)
                    erro = 0x8000104A;
                else if (ClanManager.isClanNameExist(clan._name))
                {
                    erro = 0x8000105A;
                    return;
                }
                else if (ClanManager._clans.Count > ConfigGS.maxActiveClans)
                    erro = 0x80001055;
                else if (PlayerManager.CreateClan(out clan._id, clan._name, clan.owner_id, clan._info, clan.creationDate) && PlayerManager.updateAccountGold(p.player_id, p._gp - ConfigGS.minCreateGold))
                {
                    clan.BestPlayers.SetDefault();
                    p.clanDate = clan.creationDate;
                    if (ComDiv.updateDB("accounts", "player_id", p.player_id, new string[] { "clanaccess", "clandate", "clan_id" }, 1, clan.creationDate, clan._id))
                    {
                        if (clan._id > 0)
                        {
                            p.clanId = clan._id;
                            p.clanAccess = 1;
                            ClanManager.AddClan(clan);
                            SEND_CLAN_INFOS.Load(clan, 0);
                            p._gp -= ConfigGS.minCreateGold;
                        }
                        else erro = 0x8000104B;
                    }
                    else erro = 0x80001048;
                }
                else erro = 0x80001048;
                _client.SendPacket(new CLAN_CREATE_PAK(erro, clan, p));
            }
            catch (Exception ex)
            {
                Logger.warning("[CLAN_CREATE_REC] " + ex.ToString());
            }
            /*
             * 80001055 - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_OVERFLOW
             * 8000105C - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_ALREADY
             * 8000105A - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_OVERLAPPING
             * 80001048 - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING
             * Padrão: STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_ADMIN
             */
        }
    }
}
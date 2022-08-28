using Core;
using Core.managers;
using Core.models.account.clan;
using Core.models.enums;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_CLIENT_ENTER_REC : ReceiveGamePacket
    {
        private int id;
        public CLAN_CLIENT_ENTER_REC(GameClient client, byte[] data)
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
                Room room = p._room;
                if (room != null)
                {
                    room.changeSlotState(p._slotId, SLOT_STATE.CLAN, false);
                    room.StopCountDown(p._slotId);
                    room.updateSlotsInfo();
                }
                Clan clan = ClanManager.getClan(p.clanId);
                if (p.clanId == 0 && p.player_name.Length > 0)
                    id = PlayerManager.getRequestClanId(p.player_id);
                _client.SendPacket(new CLAN_CLIENT_ENTER_PAK(id > 0 ? id : clan._id, p.clanAccess));
                if (clan._id > 0 && id == 0)
                    _client.SendPacket(new CLAN_DETAIL_INFO_PAK(0, clan));
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_CLIENT_ENTER_REC: " + ex.ToString());
            }
        }
    }
}